namespace Shrinkr.Infrastructure
{
    using System;
    using System.Collections.Generic;

    public interface IHttp
    {
        HttpResponse Get(string url, string userAgent, int timeout, bool requestCompressed, int maximumRedirects, string userName, string password, string contentType, IDictionary<string, string> headers, IDictionary<string, string> cookies);

        void GetAsync(string url, string userAgent, int timeout, bool requestCompressed, int maximumRedirects, string userName, string password, string contentType, IDictionary<string, string> headers, IDictionary<string, string> cookies, Action<HttpResponse> onComplete, Action<Exception> onError);

        HttpResponse Post(string url, string data, string userAgent, int timeout, bool requestCompressed, int maximumRedirects, string userName, string password, string contentType, IDictionary<string, string> headers, IDictionary<string, string> cookies);

        void PostAsync(string url, IDictionary<string, string> formFields, string userAgent, int timeout, bool requestCompressed, int maximumRedirects, string userName, string password, string contentType, IDictionary<string, string> headers, IDictionary<string, string> cookies, Action<HttpResponse> onComplete, Action<Exception> onError);

        void PostAsync(string url, string data, string userAgent, int timeout, bool requestCompressed, int maximumRedirects, string userName, string password, string contentType, IDictionary<string, string> headers, IDictionary<string, string> cookies, Action<HttpResponse> onComplete, Action<Exception> onError);
    }
}