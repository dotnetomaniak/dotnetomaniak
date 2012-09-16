using System;

namespace Kigg.Infrastructure
{
    public class ThumbalizrThumbnail : IThumbnail
    {
        private readonly string _key;
        private readonly string _baseUrl;
        private readonly IHttpForm _httpForm;

        public ThumbalizrThumbnail(string key, string baseUrl, IHttpForm httpForm)
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
            //5e0771e89971f9017c6d438b6c59453d
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            int size = inSize == ThumbnailSize.Small ? 111 : 250;

            return "{0}?api_key={1}&url={2}&width={3}".FormatWith(_baseUrl, _key, url, size);
        }

        public void Capture(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            _httpForm.GetAsync(new HttpFormGetRequest {Url = url});
        }

    }
}
