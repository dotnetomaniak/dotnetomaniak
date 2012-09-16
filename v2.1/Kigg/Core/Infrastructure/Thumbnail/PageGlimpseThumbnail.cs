namespace Kigg.Infrastructure
{
    public class PageGlimpseThumbnail : IThumbnail
    {
        private readonly string PreviewNotAvailable = "{0}/Assets/Images/pg-preview-na.jpg".FormatWith(IoC.Resolve<IConfigurationSettings>().RootUrl);

        private readonly string _key;
        private readonly string _baseUrl;
        private readonly IHttpForm _httpForm;

        public PageGlimpseThumbnail(string key, string baseUrl, IHttpForm httpForm)
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

            string size = inSize == ThumbnailSize.Small ? "small" : "medium";

            return "{0}?devkey={1}&url={2}&size={3}&root=no&nothumb={4}".FormatWith(_baseUrl, _key, url, size, PreviewNotAvailable);
        }

        public void Capture(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            string requestUrl = "{0}/request?devkey={1}&url={2}".FormatWith(_baseUrl, _key, url);

            _httpForm.GetAsync(requestUrl);
        }
    }
}