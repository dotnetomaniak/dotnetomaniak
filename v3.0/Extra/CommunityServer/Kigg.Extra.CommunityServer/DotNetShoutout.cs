using System.Xml;

using CommunityServer.Blogs.Components;
using CommunityServer.Components;

namespace Kigg.Extra.CommunityServer
{
    public class DotNetShoutout : ICSModule
    {
        private string _borderColor;
        private string _textBackColor;
        private string _textForeColor;
        private string _countBackColor;
        private string _countForeColor;

        public void Init(CSApplication csa, XmlNode node)
        {
            _borderColor = GetValueFrom(node, "borderColor");
            _textBackColor = GetValueFrom(node, "textBackColor");
            _textForeColor = GetValueFrom(node, "textForeColor");
            _countBackColor = GetValueFrom(node, "countBackColor");
            _countForeColor = GetValueFrom(node, "countForeColor");

            csa.PreRenderPost += OnPreRenderPost;
        }

        private void OnPreRenderPost(IContent content, CSPostEventArgs e)
        {
            if (e.ApplicationType == ApplicationType.Weblog)
            {
                WeblogPost post = content as WeblogPost;

                if ((post != null) && ((post.BlogPostType == BlogPostType.Post) || (post.BlogPostType == BlogPostType.Article)))
                {
                    content.FormattedBody = string.Concat(content.FormattedBody, GenerateCounter(post));
                }
            }
        }

        public string GenerateCounter(WeblogPost post)
        {
            string url = Globals.FullPath(BlogUrls.Instance().Post(post));
            string title = post.Subject;

            return DotNetShoutoutCounterGenerator.Generate(url, title, _borderColor, _textBackColor, _textForeColor, _countBackColor, _countForeColor);
        }

        private static string GetValueFrom(XmlNode node, string attributeName)
        {
            XmlNode attribute = node.Attributes.GetNamedItem(attributeName);

            return (attribute == null) ? string.Empty : (string.IsNullOrEmpty(attribute.Value) ? string.Empty : attribute.Value);
        }
    }
}