namespace Shrinkr.Infrastructure
{
    using System.Collections.Generic;

    using DomainObjects;

    public class Settings
    {
        public Settings(bool redirectPermanently, int urlPerPage, BaseType baseType, ApiSettings api, ThumbnailSettings thumbnail, GoogleSafeBrowsingSettings google, TwitterSettings twitter, IEnumerable<User> defaultUsers)
        {
            Check.Argument.IsNotZeroOrNegative(urlPerPage, "urlPerPage");
            Check.Argument.IsNotNull(api, "api");
            Check.Argument.IsNotNull(thumbnail, "thumbnail");
            Check.Argument.IsNotNull(google, "google");

            RedirectPermanently = redirectPermanently;
            UrlPerPage = urlPerPage;
            BaseType = baseType;
            Api = api;
            Thumbnail = thumbnail;
            Google = google;
            Twitter = twitter;
            DefaultUsers = defaultUsers ?? new User[0];
        }

        internal Settings()
        {
        }

        public bool RedirectPermanently
        {
            get;
            internal set;
        }

        public int UrlPerPage
        {
            get;
            internal set;
        }

        public BaseType BaseType
        {
            get;
            internal set;
        }

        public ApiSettings Api
        {
            get;
            internal set;
        }

        public ThumbnailSettings Thumbnail
        {
            get;
            internal set;
        }

        public GoogleSafeBrowsingSettings Google
        {
            get;
            private set;
        }

        public TwitterSettings Twitter
        {
            get;
            internal set;
        }

        public IEnumerable<User> DefaultUsers
        {
            get;
            internal set;
        }
    }
}