namespace Shrinkr.Infrastructure
{
    using Extensions;

    public class PageGlimpseThumbnail : IThumbnail
    {
        private readonly Settings settings;
        private readonly IHttp http;

        public PageGlimpseThumbnail(Settings settings, IHttp http)
        {
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotNull(http, "http");

            this.settings = settings;
            this.http = http;
        }

        public string UrlFor(string url, ThumbnailSize size)
        {
            Check.Argument.IsNotNullOrEmpty(url, "shortUrl");

            return "{0}?devkey={1}&url={2}&size={3}&root=no".FormatWith(settings.Thumbnail.EndPoint, settings.Thumbnail.ApiKey, url, size.ToString().ToLower(Culture.Invariant));
        }

        public void Capture(string url)
        {
            Check.Argument.IsNotNullOrEmpty(url, "shortUrl");

            string requestUrl = "{0}/request?devkey={1}&url={2}".FormatWith(settings.Thumbnail.EndPoint, settings.Thumbnail.ApiKey, url);

            http.GetAsync(requestUrl);
        }
    }
}