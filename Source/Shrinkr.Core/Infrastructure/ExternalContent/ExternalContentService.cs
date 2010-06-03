namespace Shrinkr.Infrastructure
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;

    public class ExternalContentService : IExternalContentService
    {
        private static readonly string keyPrefix = typeof(IExternalContentService).FullName;
        private static readonly Regex htmlTitleExpression = new Regex(@"(?<=<title.*>)([\s\S]*)(?=</title>)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private readonly IHttp http;
        private readonly ICacheManager cacheManager;

        public ExternalContentService(IHttp http, ICacheManager cacheManager)
        {
            Check.Argument.IsNotNull(http, "http");
            Check.Argument.IsNotNull(cacheManager, "cacheManager");

            this.http = http;
            this.cacheManager = cacheManager;
        }

        public ExternalContent Retrieve(string url)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");

            string key = keyPrefix + "-" + url;

            return cacheManager.GetOrCreate(key, () => Get(url));
        }

        private ExternalContent Get(string url)
        {
            HttpResponse httpResponse = null;
            WebException exception = null;

            RetryPolicy.Retry(
                                () =>
                                {
                                    try
                                    {
                                        httpResponse = http.Get(url);
                                    }
                                    catch (WebException e)
                                    {
                                        exception = e;
                                    }
                                },
                                () =>
                                {
                                    if (httpResponse != null)
                                    {
                                        return true;
                                    }

                                    HttpWebResponse response = exception.Response as HttpWebResponse;

                                    return (response != null) && (response.StatusCode >= HttpStatusCode.OK && response.StatusCode < HttpStatusCode.Ambiguous);
                                },
                                3,
                                TimeSpan.FromSeconds(1));

            if (exception != null)
            {
                throw exception;
            }

            Match match = htmlTitleExpression.Match(httpResponse.Content);

            string title = !string.IsNullOrWhiteSpace(match.Value) ? match.Value.Trim() : TextMessages.CouldNotRetrieveTheTitle;

            return new ExternalContent(title, httpResponse.Content);
        }
    }
}