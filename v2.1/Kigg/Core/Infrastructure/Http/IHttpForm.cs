namespace Kigg.Infrastructure
{
    using System;
    using System.Collections.Specialized;

    public interface IHttpForm
    {
        string Get(string url);

        string Get(string url, NameValueCollection headers);

        void GetAsync(string url);

        void GetAsync(string url, Action<string> onComplete, Action<Exception> onError);

        void GetAsync(string url, NameValueCollection headers);

        void GetAsync(string url, NameValueCollection headers, Action<string> onComplete, Action<Exception> onError);

        string Post(string url, NameValueCollection formFields);

        string Post(string url, NameValueCollection headers, NameValueCollection formFields);

        string Post(string url, string rawData);

        string Post(string url, NameValueCollection headers, string rawData);

        void PostAsync(string url, NameValueCollection formFields);

        void PostAsync(string url, NameValueCollection formFields, Action<string> onComplete, Action<Exception> onError);

        void PostAsync(string url, NameValueCollection headers, NameValueCollection formFields);

        void PostAsync(string url, NameValueCollection headers, NameValueCollection formFields, Action<string> onComplete, Action<Exception> onError);

        void PostAsync(string url, string rawData);

        void PostAsync(string url, NameValueCollection headers, string rawData);

        void PostAsync(string url, NameValueCollection headers, string rawData, Action<string> onComplete, Action<Exception> onError);
    }
}