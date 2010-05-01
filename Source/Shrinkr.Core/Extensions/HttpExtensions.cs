namespace Shrinkr.Infrastructure
{
    using System;
    using System.Collections.Generic;

    public static class HttpExtensions
    {
        public static HttpResponse Get(this IHttp instance, string url)
        {
            Check.Argument.IsNotNull(instance, "instance");

            return instance.Get(url, Http.DefaultUserAgent, Http.DefaultTimeout, Http.DefaultRequestCompressed, Http.DefaultMaximumRedirects, null, null, null, null, null);
        }

        public static void GetAsync(this IHttp instance, string url)
        {
            Check.Argument.IsNotNull(instance, "instance");

            instance.GetAsync(url, Http.DefaultUserAgent, Http.DefaultTimeout, Http.DefaultRequestCompressed, Http.DefaultMaximumRedirects, null, null, null, null, null, null, null);
        }

        public static void GetAsync(this IHttp instance, string url, Action<HttpResponse> onComplete)
        {
            Check.Argument.IsNotNull(instance, "instance");

            instance.GetAsync(url, Http.DefaultUserAgent, Http.DefaultTimeout, Http.DefaultRequestCompressed, Http.DefaultMaximumRedirects, null, null, null, null, null, onComplete, null);
        }

        public static void PostAsync(this IHttp instance, string url, string userName, string password, IDictionary<string, string> formFields)
        {
            Check.Argument.IsNotNull(instance, "instance");

            instance.PostAsync(url, formFields, Http.DefaultUserAgent, Http.DefaultTimeout, Http.DefaultRequestCompressed, Http.DefaultMaximumRedirects, userName, password, null, null, null, null, null);
        }
    }
}