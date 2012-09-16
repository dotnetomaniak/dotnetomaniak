namespace Kigg.Infrastructure
{
    using System.Collections.Specialized;

    public class HttpFormResponse
    {
        public HttpFormResponse()
        {
            Headers = new NameValueCollection();
            Cookies = new NameValueCollection();
        }

        public string Response
        {
            get;
            set;
        }

        public NameValueCollection Headers
        {
            get;
            set;
        }

        public NameValueCollection Cookies
        {
            get;
            set;
        }
    }
}