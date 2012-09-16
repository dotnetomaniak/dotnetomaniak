namespace Kigg.Infrastructure
{
    using System;
    using System.Collections.Specialized;

    // Supports both Akismet and TypePad
    public class GenericExternalSpamProtection : BaseSpamProtection
    {
        private readonly IConfigurationSettings _settings;
        private readonly IHttpForm _httpForm;

        private readonly string _name;
        private readonly string _apiKey;
        private readonly string _version;

        private readonly string _baseUrl;

        private bool _isValidApiKey;

        private string _checkUrl;
        private string _submitUrl;
        private string _falsePositiveUrl;

        public GenericExternalSpamProtection(string name, string baseUrl, string apiKey, string version, IConfigurationSettings settings, IHttpForm httpForm)
        {
            Check.Argument.IsNotEmpty(name, "name");
            Check.Argument.IsNotEmpty(baseUrl, "baseUrl");
            Check.Argument.IsNotEmpty(apiKey, "apiKey");
            Check.Argument.IsNotEmpty(version, "version");
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotNull(httpForm, "httpForm");

            _name = name;
            _baseUrl = baseUrl;
            _apiKey = apiKey;
            _version = version;

            _settings = settings;
            _httpForm = httpForm;
        }

        public GenericExternalSpamProtection(string name, string baseUrl, string apiKey, string version, IConfigurationSettings settings) : this(name, baseUrl, apiKey, version, settings, new HttpForm("{0}/{1} | {2}/{3}".FormatWith(settings.SiteTitle, typeof(GenericExternalSpamProtection).Assembly.GetName().Version, name, version), 15000, false, 8))
        {
        }

        public override bool IsSpam(SpamCheckContent spamCheckContent)
        {
            Check.Argument.IsNotNull(spamCheckContent, "spamCheckContent");

            EnsureValidApiKey();

            string response = _httpForm.Post(_checkUrl, PrepareFormFieldsFrom(spamCheckContent));
            bool isSpam = ToBool(response);

            // This protection does not think it is spam so forward it to next handler (If there is any)
            if ((!isSpam) && (NextHandler != null))
            {
                isSpam = NextHandler.IsSpam(spamCheckContent);
            }

            return isSpam;
        }

        public override void IsSpam(SpamCheckContent spamCheckContent, Action<string, bool> callback)
        {
            Check.Argument.IsNotNull(spamCheckContent, "spamCheckContent");
            Check.Argument.IsNotNull(callback, "callback");

            EnsureValidApiKey();

            _httpForm.PostAsync(
                                    _checkUrl,
                                    PrepareFormFieldsFrom(spamCheckContent),
                                    response =>
                                    {
                                        bool isSpam = ToBool(response);

                                        // This protection does not think it is spam so forward it to next handler (If there is any)
                                        if ((!isSpam) && (NextHandler != null))
                                        {
                                            NextHandler.IsSpam(spamCheckContent, callback);
                                        }
                                        else
                                        {
                                            callback(_name, isSpam);
                                        }
                                    },
                                    e =>
                                    {
                                        // When exception occurs try next handler
                                        if (NextHandler != null)
                                        {
                                            NextHandler.IsSpam(spamCheckContent, callback);
                                        }
                                        else
                                        {
                                            callback(_name, false);
                                        }
                                    });
        }

        public void MarkAsSpam(SpamCheckContent checkContent)
        {
            Check.Argument.IsNotNull(checkContent, "checkContent");

            EnsureValidApiKey();

            _httpForm.PostAsync(_submitUrl, PrepareFormFieldsFrom(checkContent));
        }

        public void MarkAsFalsePositive(SpamCheckContent checkContent)
        {
            Check.Argument.IsNotNull(checkContent, "checkContent");

            EnsureValidApiKey();

            _httpForm.PostAsync(_falsePositiveUrl, PrepareFormFieldsFrom(checkContent));
        }

        private static bool ToBool(string response)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(response) && !bool.TryParse(response, out result))
            {
                result = false;
            }

            return result;
        }

        private void EnsureValidApiKey()
        {
            if (!_isValidApiKey)
            {
                _isValidApiKey = IsValidApiKey();

                if (!_isValidApiKey)
                {
                    throw new InvalidOperationException("Specified api key is not valid.");
                }

                BuildUrls();
            }
        }

        private void BuildUrls()
        {
            _checkUrl = "http://{0}.{1}/{2}/comment-check".FormatWith(_apiKey, _baseUrl, _version);
            _submitUrl = "http://{0}.{1}/{2}/submit-spam".FormatWith(_apiKey, _baseUrl, _version);
            _falsePositiveUrl = "http://{0}.{1}/{2}/submit-ham".FormatWith(_apiKey, _baseUrl, _version);
        }

        private bool IsValidApiKey()
        {
            NameValueCollection parameters = new NameValueCollection
                                                 {
                                                     { "key", _apiKey.UrlEncode() },
                                                     { "blog", _settings.RootUrl.UrlEncode() }
                                                 };

            string response = _httpForm.Post("http://{0}/{1}/verify-key".FormatWith(_baseUrl, _version), parameters);

            return string.Compare(response, "valid", StringComparison.OrdinalIgnoreCase) == 0;
        }

        private NameValueCollection PrepareFormFieldsFrom(SpamCheckContent checkContent)
        {
            NameValueCollection formFields = new NameValueCollection { { "blog", _settings.RootUrl } };

            if (checkContent.Extra.Count > 0)
            {
                foreach (string key in checkContent.Extra)
                {
                    formFields.Add(key.UrlEncode(), checkContent.Extra[key].UrlEncode());
                }
            }

            Action<string, string> addIfSpecified = (key, value) =>
                                                    {
                                                        if (!string.IsNullOrEmpty(value))
                                                        {
                                                            formFields.Add(key.UrlEncode(), value.UrlEncode());
                                                        }
                                                    };

            addIfSpecified("user_ip", checkContent.UserIPAddress);
            addIfSpecified("user_agent", checkContent.UserAgent);
            addIfSpecified("comment_author", checkContent.UserName);
            addIfSpecified("comment_content", checkContent.Content);
            addIfSpecified("referer", checkContent.UrlReferer);
            addIfSpecified("permalink", checkContent.Url);
            addIfSpecified("comment_type", checkContent.ContentType);

            return formFields;
        }
    }
}