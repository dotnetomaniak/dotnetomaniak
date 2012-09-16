namespace Kigg.Infrastructure.HtmlAgilityPack
{
    using System;

    using Document = global::HtmlAgilityPack.HtmlDocument;
    using Node = global::HtmlAgilityPack.HtmlNode;

    public class HtmlSanitizer : IHtmlSanitizer
    {
        private static readonly string[] _allowedElements = "p|#text|strong|em|ol|ul|li|blockquote|pre|code|a".Split('|');

        public virtual string Sanitize(string html)
        {
            Document doc = new Document();
            doc.LoadHtml(html);

            foreach (var childNode in doc.DocumentNode.ChildNodes)
            {
                RemoveUnwantedNodes(childNode);
            }

            var nodes = doc.DocumentNode.SelectNodes("//a[starts-with(@href, 'javascript')]|//a[starts-with(@href, 'jscript')]|//a[starts-with(@href, 'vbscript')]");

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    node.SetAttributeValue("href", "protected");
                }
            }

            nodes = doc.DocumentNode.SelectNodes("//a");

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    node.SetAttributeValue("rel", "nofollow");
                }
            }

            nodes = doc.DocumentNode.SelectNodes("//*[@onclick or @onmouseover or @onfocus or @onblur or @onmouseout or @ondoubleclick or @onload or @onunload or @style or @class or @id or @title]");

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    node.Attributes.Remove("onclick");
                    node.Attributes.Remove("onmouseover");
                    node.Attributes.Remove("onfocus");
                    node.Attributes.Remove("onblur");
                    node.Attributes.Remove("onmouseout");
                    node.Attributes.Remove("ondoubleclick");
                    node.Attributes.Remove("onload");
                    node.Attributes.Remove("onunload");

                    node.Attributes.Remove("style");
                    node.Attributes.Remove("class");
                    node.Attributes.Remove("id");
                    node.Attributes.Remove("title");
                }
            }

            return doc.DocumentNode.InnerHtml;
        }

        private static void RemoveUnwantedNodes(Node node)
        {
            if (Array.IndexOf(_allowedElements, node.Name) > -1)
            {
                foreach (Node childNode in node.ChildNodes)
                {
                    RemoveUnwantedNodes(childNode);
                }
            }
            else
            {
                node.ParentNode.RemoveChild(node, false);
            }
        }
    }
}