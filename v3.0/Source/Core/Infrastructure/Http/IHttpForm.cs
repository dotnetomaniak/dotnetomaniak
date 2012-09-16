namespace Kigg.Infrastructure
{
    using System;

    public interface IHttpForm
    {
        HttpFormResponse Get(HttpFormGetRequest getRequest);

        void GetAsync(HttpFormGetRequest getRequest);

        void GetAsync(HttpFormGetRequest getRequest, Action<HttpFormResponse> onComplete, Action<Exception> onError);

        HttpFormResponse Post(HttpFormPostRequest postRequest);

        HttpFormResponse Post(HttpFormPostRawRequest postRequest);

        void PostAsync(HttpFormPostRequest postRequest);

        void PostAsync(HttpFormPostRequest postRequest, Action<HttpFormResponse> onComplete, Action<Exception> onError);

        void PostAsync(HttpFormPostRawRequest postRequest);

        void PostAsync(HttpFormPostRawRequest postRequest, Action<HttpFormResponse> onComplete, Action<Exception> onError);
    }
}