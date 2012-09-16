namespace Kigg.Infrastructure
{
    using System;
    using System.Collections.Specialized;
    using System.Xml.Linq;

    public class DefensioSpamProtection : BaseSpamProtection
    {
        private const string Source = "Defensio";

        private readonly IConfigurationSettings _settings;
        private readonly IHttpForm _httpForm;

        private readonly string _apiKey;
        private readonly string _version;

        private bool _isValidApiKey;

        private string _checkUrl;

        public DefensioSpamProtection(string apiKey, string version, IConfigurationSettings settings, IHttpForm httpForm)
        {
            Check.Argument.IsNotEmpty(apiKey, "apiKey");
            Check.Argument.IsNotEmpty(apiKey, "version");
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotNull(httpForm, "httpForm");

            _apiKey = apiKey;
            _version = version;

            _settings = settings;
            _httpForm = httpForm;
        }

        public override bool IsSpam(SpamCheckContent spamCheckContent)
        {
            Check.Argument.IsNotNull(spamCheckContent, "spamCheckContent");

            EnsureValidApiKey();

            string response = _httpForm.Post(_checkUrl, PrepareFormFields(spamCheckContent));

            bool isSpam = IsMatch(response, "spam", "true");

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
                                    PrepareFormFields(spamCheckContent),
                                    response =>
                                                   {
                                                       bool isSpam = IsMatch(response, "spam", "true");

                                                       // Defensio does not think it is spam so forward it to next handler (If there is any)
                                                       if ((!isSpam) && (NextHandler != null))
                                                       {
                                                           NextHandler.IsSpam(spamCheckContent, callback);
                                                       }
                                                       else
                                                       {
                                                           callback(Source, isSpam);
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
                                               callback(Source, false);
                                           }
                                        });
        }

        private static bool IsMatch(string xml, string tagName, string tagValue)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                XDocument doc = XDocument.Parse(xml);
                XElement root = doc.Element("defensio-result");

                if (root != null)
                {
                    XElement target = root.Element(tagName);

                    if (target != null)
                    {
                        return string.Compare(target.Value, tagValue, StringComparison.OrdinalIgnoreCase) == 0;
                    }
                }
            }

            return false;
        }

        private NameValueCollection PrepareFormFields(SpamCheckContent spamCheckContent)
        {
            NameValueCollection formFields = new NameValueCollection();

            Action<string, string> addIfSpecified = (key, value) =>
                                                    {
                                                        if (!string.IsNullOrEmpty(value))
                                                        {
                                                            formFields.Add(key, value);
                                                        }
                                                    };

            addIfSpecified("owner-url", _settings.RootUrl);
            addIfSpecified("user-ip", spamCheckContent.UserIPAddress);
            addIfSpecified("article-date", SystemTime .Now().ToString("yyyy/MM/dd", Constants.CurrentCulture));
            addIfSpecified("comment-author", spamCheckContent.UserName);
            addIfSpecified("comment-type", (string.Compare(spamCheckContent.ContentType, "comment", StringComparison.OrdinalIgnoreCase) == 0) ? "comment" : "other");
            addIfSpecified("comment-spamCheckContent", spamCheckContent.Content);
            addIfSpecified("permalink", spamCheckContent.Url);
            addIfSpecified("referrer", spamCheckContent.UrlReferer);

            return formFields;
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
            _checkUrl = "http://api.defensio.com/app/{1}/audit-comment/{0}.xml".FormatWith(_apiKey, _version);
        }

        private bool IsValidApiKey()
        {
            string response = _httpForm.Post("http://api.defensio.com/app/{1}/validate-key/{0}.xml".FormatWith(_apiKey, _version), new NameValueCollection { { "owner-url", _settings.RootUrl } });

            return IsMatch(response, "status", "success");
        }
    }
}