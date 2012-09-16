namespace Kigg.Web
{
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Net;

    using Infrastructure;

    public class reCAPTCHAValidator
    {
        private readonly string _verifyUrl;
        private readonly string _insecureHost;
        private readonly string _secureHost;

        private readonly string _privateKey;
        private readonly string _publicKey;

        private readonly string _challengeInputName;
        private readonly string _responseInputName;

        private readonly IHttpForm _httpForm;

        public reCAPTCHAValidator(string verifyUrl, string insecureHost, string secureHost, string privateKey, string publicKey, string challengeInputName, string responseInputName, IHttpForm httpFrom)
        {
            Check.Argument.IsNotInvalidWebUrl(verifyUrl, "verifyUrl");
            Check.Argument.IsNotInvalidWebUrl(insecureHost, "insecureHost");
            Check.Argument.IsNotInvalidWebUrl(secureHost, "secureHost");
            Check.Argument.IsNotEmpty(privateKey, "privateKey");
            Check.Argument.IsNotEmpty(publicKey, "publicKey");
            Check.Argument.IsNotEmpty(challengeInputName, "challengeInputName");
            Check.Argument.IsNotEmpty(responseInputName, "responseInputName");
            Check.Argument.IsNotNull(httpFrom, "httpFrom");

            _verifyUrl = verifyUrl;
            _insecureHost = insecureHost;
            _secureHost = secureHost;
            _privateKey = privateKey;
            _publicKey = publicKey;
            _challengeInputName = challengeInputName;
            _responseInputName = responseInputName;
            _httpForm = httpFrom;
        }

        public string InsecureHost
        {
            [DebuggerStepThrough]
            get { return _insecureHost; }
        }

        public string SecureHost
        {
            [DebuggerStepThrough]
            get { return _secureHost; }
        }

        public string PublicKey
        {
            [DebuggerStepThrough]
            get { return _publicKey; }
        }

        public string ChallengeInputName
        {
            [DebuggerStepThrough]
            get { return _challengeInputName; }
        }

        public string ResponseInputName
        {
            [DebuggerStepThrough]
            get { return _responseInputName; }
        }

        public virtual bool Validate(string fromIPAddress, string challenge, string response)
        {
            Check.Argument.IsNotEmpty(fromIPAddress, "fromIPAddress");
            Check.Argument.IsNotEmpty(challenge, "challenge");
            Check.Argument.IsNotEmpty(response, "response");

            try
            {
                string[] result = _httpForm.Post(
                                                    new HttpFormPostRequest
                                                        {
                                                            Url = _verifyUrl,
                                                            FormFields =    new NameValueCollection
                                                                            {
                                                                                { "privatekey", _privateKey.UrlEncode() },
                                                                                { "remoteip", fromIPAddress.UrlEncode() },
                                                                                { "challenge", challenge.UrlEncode() },
                                                                                { "response", response.UrlEncode() }
                                                                            }
                                                        }
                                                ).Response.Split();

                if (result.Length > 0)
                {
                    bool isValid;

                    if (!bool.TryParse(result[0], out isValid))
                    {
                        isValid = false;
                    }

                    return isValid;
                }
            }
            catch (WebException e)
            {
                Log.Exception(e);
            }

            return true;
        }
    }
}