namespace Shrinkr.Repositories
{
    using System;

    using DomainObjects;

    public interface IShortUrlRepository : IRepository<ShortUrl>
    {
        ShortUrl GetByHash(string hash);

        ShortUrl GetByAliasName(string aliasName);

        PagedResult<ShortUrl> FindByUserId(long userId, int start, int max);

        int GetCreatedCount(DateTime fromDate, DateTime toDate);

        int GetVisitedCount(DateTime fromDate, DateTime toDate);
    }
}