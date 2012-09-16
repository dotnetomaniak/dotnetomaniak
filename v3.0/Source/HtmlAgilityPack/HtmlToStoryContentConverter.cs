namespace Kigg.Infrastructure.HtmlAgilityPack
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    using Document = global::HtmlAgilityPack.HtmlDocument;
    using Entity = global::HtmlAgilityPack.HtmlEntity;
    using Node = global::HtmlAgilityPack.HtmlNode;
    using NodeType = global::HtmlAgilityPack.HtmlNodeType;
    using TextNode = global::HtmlAgilityPack.HtmlTextNode;

    public class HtmlToStoryContentConverter : IHtmlToStoryContentConverter
    {
        private static readonly Regex TrackbackExpression = new Regex("trackback:ping=\"([^\"]+)\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly List<string> _xPaths = new List<string>();
        private readonly IHtmlSanitizer _sanitizer;
        private readonly List<string> _titleFilters = new List<string>();

        public HtmlToStoryContentConverter(IHtmlSanitizer sanitizer, string xPathsFileName, string titleFiltersFileName)
            : this(sanitizer, ReadTextFile(xPathsFileName), ReadTextFile(titleFiltersFileName))
        {
        }

        public HtmlToStoryContentConverter(IHtmlSanitizer sanitizer, string xPathsFileName)
            : this(sanitizer, ReadTextFile(xPathsFileName))
        {
        }

        public HtmlToStoryContentConverter(IHtmlSanitizer sanitizer, ICollection<string> xPaths)
        {
            Check.Argument.IsNotNull(sanitizer, "sanitizer");
            Check.Argument.IsNotEmpty(xPaths, "xPaths");
            
            _sanitizer = sanitizer;
            _xPaths.AddRange(xPaths);
        }

        public HtmlToStoryContentConverter(IHtmlSanitizer sanitizer, ICollection<string> xPaths, ICollection<string> titleFilters)
            : this (sanitizer, xPaths)
        {
            _titleFilters.AddRange(titleFilters);
        }

        public virtual StoryContent Convert(string url, string html)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");
            Check.Argument.IsNotEmpty(html, "html");

            Document doc = new Document();
            doc.LoadHtml(html);

            string title = GetTitle(doc);
            string content = GetContent(doc);

            if (!string.IsNullOrEmpty(content))
            {
                content = _sanitizer.Sanitize(content);

                if (!string.IsNullOrEmpty(content))
                {
                    content = content.WrapAt(512);
                }
            }

            string trackbackUrl = GetTrackbackUrl(html) ?? GetTrackbackUrl(url, doc);

            return new StoryContent(title, content, trackbackUrl);
        }

        private string GetTitle(Document document)
        {
            Node titleNode = document.DocumentNode.SelectSingleNode("//title");

            return (titleNode == null) ? null : titleNode.InnerText.Trim('\t').Replace(_titleFilters, string.Empty).Trim();
        }

        private static string[] ReadTextFile(string fileName)
        {
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)).Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }
        
        private static string GetTrackbackUrl(string html)
        {
            Match match = TrackbackExpression.Match(html);

            string trackbackUrl = ((match != null) && (match.Groups != null) && (match.Groups.Count > 1)) ? match.Groups[1].Value.Trim() : null;

            return trackbackUrl;
        }

        private static string GetTrackbackUrl(string url, Document document)
        {
            // First try the Typepad
            Node targetNode = document.DocumentNode.SelectSingleNode(".//div[@class=\"trackbacks-info\"]/p/span[@class=\"trackbacks-link\"]");

            if (targetNode != null)
            {
                return targetNode.InnerText;
            }

            // Then B2Evolution
            targetNode = document.DocumentNode.SelectSingleNode(".//p[@class=\"trackback_url\"]/a");

            if ((targetNode != null) && targetNode.HasAttributes)
            {
                return targetNode.GetAttributeValue("href", null);
            }

            // And at last, check wordpress
            targetNode = document.DocumentNode.SelectSingleNode("//meta[@name=\"generator\"]");

            if (targetNode != null)
            {
                string generator = targetNode.GetAttributeValue("content", string.Empty).Trim();

                if (generator.StartsWith("WordPress", StringComparison.OrdinalIgnoreCase))
                {
                    return url + (!url.EndsWith("/", StringComparison.OrdinalIgnoreCase) ? "/" : string.Empty) + "trackback/";
                }
            }

            return null;
        }

        private string GetContent(Document document)
        {
            string body = string.Empty;

            try
            {
                Node bodyNode = document.DocumentNode.SelectSingleNode("//body");
                Node contentNode = null;

                if (bodyNode != null)
                {
                    contentNode = TryToFindContentNode(bodyNode) ?? bodyNode;
                }

                if (bodyNode != null)
                {
                    using (StringWriter writer = new StringWriter(Constants.CurrentCulture))
                    {
                        ConvertTo(contentNode, writer);
                        writer.Flush();

                        body = writer.ToString();
                    }
                }
            }
            catch (NullReferenceException)
            {
            }

            return body.Trim('\t').Trim();
        }

        private Node TryToFindContentNode(Node bodyNode)
        {
            Node contentNode = null;

            foreach (string xPath in _xPaths)
            {
                contentNode = bodyNode.SelectSingleNode(xPath);

                if (contentNode != null)
                {
                    break;
                }
            }

            return contentNode;
        }

        private void ConvertTo(Node node, TextWriter outText)
        {
            switch (node.NodeType)
            {
                case NodeType.Element:
                    {
                        string nodeName = node.Name;

                        if ((string.Compare(nodeName, "p", StringComparison.OrdinalIgnoreCase) == 0) ||
                            (string.Compare(nodeName, "br", StringComparison.OrdinalIgnoreCase) == 0) ||
                            (string.Compare(nodeName, "hr", StringComparison.OrdinalIgnoreCase) == 0))
                        {
                            outText.Write("\r\n"); 
                        }
                        if (string.Compare(nodeName, "h1", StringComparison.OrdinalIgnoreCase) != 0)
                        {
                            if (node.HasChildNodes)
                            {
                                ConvertContentTo(node, outText);
                            }
                        }

                        break;
                    }

                case NodeType.Text:
                    {
                        string parentName = node.ParentNode.Name;

                        if ((string.Compare(parentName, "script", StringComparison.OrdinalIgnoreCase) == 0) ||
                            (string.Compare(parentName, "style", StringComparison.OrdinalIgnoreCase) == 0))
                        {
                            break;
                        }

                        string html = ((TextNode)node).Text;

                        if (Node.IsOverlappedClosingElement(html))
                        {
                            break;
                        }

                        DateTime parsedDateTime;

                        if (html.Trim().Length == 0 ||
                        html.Length > 2 && string.Compare(html.Substring(0, 3), "by ", StringComparison.OrdinalIgnoreCase) == 0 ||
                        DateTime.TryParse(html.Trim(), out parsedDateTime))
                        {
                            break;
                        }

                        outText.Write(Entity.DeEntitize(html));
                        break;
                    }
            }
        }

        private void ConvertContentTo(Node node, TextWriter outText)
        {
            foreach (var childNode in node.ChildNodes)
            {
                ConvertTo(childNode, outText);
            }
        }
    }
}