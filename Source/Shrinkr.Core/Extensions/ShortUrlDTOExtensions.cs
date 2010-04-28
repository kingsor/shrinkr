namespace Shrinkr.DataTransferObjects
{
    using Infrastructure;

    using Microsoft.Practices.ServiceLocation;

    public static class ShortUrlDTOExtensions
    {
        public static string SmallThumbnail(this ShortUrlDTO instance)
        {
            return Thumbnail(instance, ThumbnailSize.Small);
        }

        public static string MediumThumbnail(this ShortUrlDTO instance)
        {
            return Thumbnail(instance, ThumbnailSize.Medium);
        }

        public static string LargeThumbnail(this ShortUrlDTO instance)
        {
            return Thumbnail(instance, ThumbnailSize.Large);
        }

        private static string Thumbnail(ShortUrlDTO shortUrl, ThumbnailSize size)
        {
            Check.Argument.IsNotNull(shortUrl, "shortUrl");

            return ServiceLocator.Current.GetInstance<IThumbnail>().UrlFor(shortUrl.Url, size);
        }
    }
}