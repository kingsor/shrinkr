namespace Shrinkr.Infrastructure
{
    using DomainObjects;

    public class GoogleSafeBrowsingSpamDetector : ISpamDetector
    {
        private readonly IGoogleSafeBrowsing googleSafeBrowsing;

        public GoogleSafeBrowsingSpamDetector(IGoogleSafeBrowsing googleSafeBrowsing)
        {
            Check.Argument.IsNotNull(googleSafeBrowsing, "googleSafeBrowsing");

            this.googleSafeBrowsing = googleSafeBrowsing;
        }

        public SpamStatus CheckStatus(ShortUrl shortUrl)
        {
            Check.Argument.IsNotNull(shortUrl, "shortUrl");

            int phishingCount;
            int malwareCount;

            googleSafeBrowsing.Verify(shortUrl.Url, out phishingCount, out malwareCount);

            SpamStatus status = SpamStatus.None;

            if ((phishingCount > 0) || (malwareCount > 0))
            {
                if (phishingCount > 0)
                {
                    status |= SpamStatus.Phishing;
                }

                if (malwareCount > 0)
                {
                    status |= SpamStatus.Malware;
                }
            }
            else
            {
                status = SpamStatus.Clean;
            }

            return status;
        }
    }
}