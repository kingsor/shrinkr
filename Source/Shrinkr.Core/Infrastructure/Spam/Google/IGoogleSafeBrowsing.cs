namespace Shrinkr.Infrastructure
{
    public interface IGoogleSafeBrowsing
    {
        void Verify(string url, out int phishingCount, out int malwareCount);

        void Update();
    }
}