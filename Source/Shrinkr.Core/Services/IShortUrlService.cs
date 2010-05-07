namespace Shrinkr.Services
{
    public interface IShortUrlService
    {
        ShortUrlResult CreateWithUserName(string url, string aliasName, string ipAddress, string userName);

        ShortUrlResult CreateWithApiKey(string url, string aliasName, string ipAddress, string apiKey);

        ShortUrlResult GetByAlias(string aliasName);

        VisitResult Visit(string aliasName, string ipAddress, string browser, string referrer);

        ShortUrlListResult FindByUser(string userName, int start, int max);
    }
}