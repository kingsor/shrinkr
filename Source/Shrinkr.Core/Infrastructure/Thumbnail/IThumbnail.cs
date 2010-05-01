namespace Shrinkr.Infrastructure
{
    public interface IThumbnail
    {
        string UrlFor(string url, ThumbnailSize size);

        void Capture(string url);
    }
}