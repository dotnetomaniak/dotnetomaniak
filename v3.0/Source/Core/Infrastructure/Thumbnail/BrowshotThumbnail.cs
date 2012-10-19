

using System;

namespace Kigg.Infrastructure
{
    public class BrowshotThumbnail : IThumbnail
    {
        private readonly string _key;
        private readonly string _baseUrl;
        private readonly int _instanceId;
        private readonly IHttpForm _httpForm;

        public BrowshotThumbnail(string key, string baseUrl, int instanceId, IHttpForm httpForm)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotEmpty(baseUrl, "baseUrl");
            Check.Argument.IsNotNull(httpForm, "httpForm");

            _key = key; 
            _baseUrl = baseUrl;
            _instanceId = instanceId;
            _httpForm = httpForm;
        }

        public string For(string url, ThumbnailSize inSize)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            string size = inSize == ThumbnailSize.Small ? "width=111&height=111" : "width=250&height=250";

            return "{0}?url={1}&instance_id={2}&{3}&key={4}".FormatWith(_baseUrl, url, _instanceId, size, _key);
        }

        public void Capture(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            _httpForm.GetAsync(new HttpFormGetRequest { Url = url });
        }
    }
}