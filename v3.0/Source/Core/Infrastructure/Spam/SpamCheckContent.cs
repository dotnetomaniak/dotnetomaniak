namespace Kigg.Infrastructure
{
    using System.Collections.Specialized;
    using System.Diagnostics;

    public class SpamCheckContent
    {
        private readonly NameValueCollection _extra = new NameValueCollection();

        public string Content
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string UserIPAddress
        {
            get;
            set;
        }

        public string UserAgent
        {
            get;
            set;
        }

        public string UrlReferer
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }

        public NameValueCollection Extra
        {
            [DebuggerStepThrough]
            get
            {
                return _extra;
            }
        }
    }
}