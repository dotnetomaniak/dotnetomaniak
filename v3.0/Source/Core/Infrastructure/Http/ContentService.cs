using System.IO;

namespace Kigg.Infrastructure
{
    using System;
    using System.Collections.Generic;

    public class ContentService : IContentService
    {
        private readonly IHttpForm _httpForm;
        private readonly IHtmlToStoryContentConverter _converter;
        private readonly string _shortUrlProviderFormat;

        private readonly List<string> _blockedUrlPrefixes = new List<string>();
        private readonly IFile _fileReader;
        private readonly string _blockedUrlPrefixFilePath;

        public ContentService(IHttpForm httpForm, IHtmlToStoryContentConverter converter, IFile fileReader, string blockedUrlPrefixFilePath, string shortUrlProviderFormat)
        {
            Check.Argument.IsNotNull(httpForm, "httpForm");
            Check.Argument.IsNotNull(converter, "converter");
            Check.Argument.IsNotNull(fileReader, "fileReader");
            Check.Argument.IsNotEmpty(blockedUrlPrefixFilePath, "blockedUrlPrefixFilePath");
            Check.Argument.IsNotEmpty(shortUrlProviderFormat, "shortUrlProviderFormat");

            _httpForm = httpForm;
            _converter = converter;
            _fileReader = fileReader;
            _shortUrlProviderFormat = shortUrlProviderFormat;

            _blockedUrlPrefixFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, blockedUrlPrefixFilePath);
            ReadUrlPrefexes();
        }

        public virtual bool IsRestricted(string url)
        {
            const string Prefix = "http://www.";

            if (url.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase))
            {
                url = "http://" + url.Substring(Prefix.Length);
            }

            foreach(string prefix in _blockedUrlPrefixes)
            {
                if (url.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public virtual StoryContent Get(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            string html = _httpForm.Get(new HttpFormGetRequest{ Url = url }).Response;

            return string.IsNullOrEmpty(html) ? StoryContent.Empty : _converter.Convert(url, html);
        }

        public virtual string ShortUrl(string url)
        {
            Check.Argument.IsNotEmpty(url, "url");

            string shortUrl = _httpForm.Get(new HttpFormGetRequest { Url = _shortUrlProviderFormat.FormatWith(url.UrlEncode()) } ).Response;

            return string.IsNullOrEmpty(shortUrl) ? url : ((shortUrl.Length < url.Length) ? shortUrl : url);
        }

        private void ReadUrlPrefexes()
        {
            _blockedUrlPrefixes.Clear();
            _blockedUrlPrefixes.AddRange(_fileReader.ReadAllLine(_blockedUrlPrefixFilePath));
            _blockedUrlPrefixes.RemoveAll(prefix => string.IsNullOrEmpty(prefix));
        }
    }
}