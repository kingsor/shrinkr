namespace Shrinkr.Infrastructure
{
    using DomainObjects;

    public interface ISpamDetector
    {
        SpamStatus CheckStatus(ShortUrl shortUrl);
    }
}