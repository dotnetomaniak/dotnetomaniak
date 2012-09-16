namespace Kigg.Infrastructure
{
    using System;
    using System.Diagnostics;

    public abstract class DecoratedHttpForm : IHttpForm
    {
        private readonly IHttpForm _innerHttpForm;

        protected DecoratedHttpForm(IHttpForm innerHttpForm)
        {
            Check.Argument.IsNotNull(innerHttpForm, "innerHttpForm");

            _innerHttpForm = innerHttpForm;
        }

        [DebuggerStepThrough]
        public virtual HttpFormResponse Get(HttpFormGetRequest getRequest)
        {
            return _innerHttpForm.Get(getRequest);
        }

        [DebuggerStepThrough]
        public virtual void GetAsync(HttpFormGetRequest getRequest)
        {
            _innerHttpForm.GetAsync(getRequest);
        }

        [DebuggerStepThrough]
        public virtual void GetAsync(HttpFormGetRequest getRequest, Action<HttpFormResponse> onComplete, Action<Exception> onError)
        {
            _innerHttpForm.GetAsync(getRequest, onComplete, onError);
        }

        [DebuggerStepThrough]
        public virtual HttpFormResponse Post(HttpFormPostRequest postRequest)
        {
            return _innerHttpForm.Post(postRequest);
        }

        [DebuggerStepThrough]
        public virtual HttpFormResponse Post(HttpFormPostRawRequest postRequest)
        {
            return _innerHttpForm.Post(postRequest);
        }

        [DebuggerStepThrough]
        public virtual void PostAsync(HttpFormPostRequest postRequest)
        {
            _innerHttpForm.PostAsync(postRequest);
        }

        [DebuggerStepThrough]
        public virtual void PostAsync(HttpFormPostRequest postRequest, Action<HttpFormResponse> onComplete, Action<Exception> onError)
        {
            _innerHttpForm.PostAsync(postRequest, onComplete, onError);
        }

        [DebuggerStepThrough]
        public virtual void PostAsync(HttpFormPostRawRequest postRequest)
        {
            _innerHttpForm.PostAsync(postRequest);
        }

        [DebuggerStepThrough]
        public virtual void PostAsync(HttpFormPostRawRequest postRequest, Action<HttpFormResponse> onComplete, Action<Exception> onError)
        {
            _innerHttpForm.PostAsync(postRequest, onComplete, onError);
        }
    }
}