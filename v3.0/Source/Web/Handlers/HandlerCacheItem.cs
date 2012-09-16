namespace Kigg.Web
{
    public class HandlerCacheItem
    {
        private string _etag;

        public string Content
        {
            get;
            set;
        }

        public string ETag
        {
            get
            {
                if (string.IsNullOrEmpty(_etag) && !string.IsNullOrEmpty(Content))
                {
                    _etag = Content.Hash();
                }

                return _etag;
            }
        }
    }
}