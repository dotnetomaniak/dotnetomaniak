namespace Kigg.Infrastructure
{
    public class WebSnaprThumbnail : IThumbnail
    {
        private readonly string _key;
        private readonly string _baseUrl;
        private readonly IHttpForm _httpForm;

        public WebSnaprThumbnail(string key, string baseUrl, IHttpForm httpForm)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotEmpty(baseUrl, "baseUrl");
            Check.Argument.IsNotNull(httpForm, "httpForm");

            _key = key;
            _baseUrl = baseUrl;
            _httpForm = httpForm;
        }

        public string For(string url, ThumbnailSize inSize)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            string size = inSize == ThumbnailSize.Small ? "T" : "S";

            return "{0}/?key={1}&url={2}&size={3}".FormatWith(_baseUrl, _key, url, size);
        }

        public void Capture(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            _httpForm.GetAsync(For(url, ThumbnailSize.Small));
        }
    }
}