using System;

using Graffiti.Core;

namespace Kigg.Extra.Graffiti
{
    [Chalk("DotNetShoutout")]
    public class DotNetShoutout
    {
        public string Counter(Post post)
        {
            return Counter(post, null, null, null, null, null);
        }

        public string Counter(Post post, string borderColor, string textBackColor, string textForeColor, string countBackColor, string countForeColor)
        {
            if (post == null)
            {
                throw new ArgumentNullException("post", "post cannot be null.");
            }

            string url = new Macros().FullUrl(post.Url);
            string title = post.Title;

            return DotNetShoutoutCounterGenerator.Generate(url, title, borderColor, textBackColor, textForeColor, countBackColor, countForeColor);
        }
    }
}