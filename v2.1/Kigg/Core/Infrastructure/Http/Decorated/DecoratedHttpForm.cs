namespace Kigg.Infrastructure
{
    using System;
    using System.Collections.Specialized;

    public abstract class DecoratedHttpForm : IHttpForm
    {
        private readonly IHttpForm _innerHttpForm;

        protected DecoratedHttpForm(IHttpForm innerHttpForm)
        {
            Check.Argument.IsNotNull(innerHttpForm, "innerHttpForm");

            _innerHttpForm = innerHttpForm;
        }

        public virtual string Get(string url)
        {
            return _innerHttpForm.Get(url);
        }

        public virtual string Get(string url, NameValueCollection headers)
        {
            return _innerHttpForm.Get(url, headers);
        }

        public virtual void GetAsync(string url)
        {
            _innerHttpForm.GetAsync(url);
        }

        public virtual void GetAsync(string url, Action<string> onComplete, Action<Exception> onError)
        {
            _innerHttpForm.GetAsync(url, onComplete, onError);
        }

        public virtual void GetAsync(string url, NameValueCollection headers)
        {
            _innerHttpForm.GetAsync(url, headers);
        }

        public virtual void GetAsync(string url, NameValueCollection headers, Action<string> onComplete, Action<Exception> onError)
        {
            _innerHttpForm.GetAsync(url, headers, onComplete, onError);
        }

        public virtual string Post(string url, NameValueCollection formFields)
        {
            return _innerHttpForm.Post(url, formFields);
        }

        public virtual string Post(string url, NameValueCollection headers, NameValueCollection formFields)
        {
            return _innerHttpForm.Post(url, headers, formFields);
        }

        public virtual string Post(string url, string rawData)
        {
            return _innerHttpForm.Post(url, rawData);
        }

        public virtual string Post(string url, NameValueCollection headers, string rawData)
        {
            return _innerHttpForm.Post(url, headers, rawData);
        }

        public virtual void PostAsync(string url, NameValueCollection formFields)
        {
            _innerHttpForm.PostAsync(url, formFields);
        }

        public virtual void PostAsync(string url, NameValueCollection formFields, Action<string> onComplete, Action<Exception> onError)
        {
            _innerHttpForm.PostAsync(url, formFields, onComplete, onError);
        }

        public virtual void PostAsync(string url, NameValueCollection headers, NameValueCollection formFields)
        {
            _innerHttpForm.PostAsync(url, headers, formFields);
        }

        public virtual void PostAsync(string url, NameValueCollection headers, NameValueCollection formFields, Action<string> onComplete, Action<Exception> onError)
        {
            _innerHttpForm.PostAsync(url, headers, formFields, onComplete, onError);
        }

        public virtual void PostAsync(string url, string rawData)
        {
            _innerHttpForm.PostAsync(url, rawData);
        }

        public virtual void PostAsync(string url, NameValueCollection headers, string rawData)
        {
            _innerHttpForm.PostAsync(url, headers, rawData);
        }

        public virtual void PostAsync(string url, NameValueCollection headers, string rawData, Action<string> onComplete, Action<Exception> onError)
        {
            _innerHttpForm.PostAsync(url, headers, rawData, onComplete, onError);
        }
    }
}