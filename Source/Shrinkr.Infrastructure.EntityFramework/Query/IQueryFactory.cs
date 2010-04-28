namespace Shrinkr.Infrastructure.EntityFramework
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public interface IQueryFactory
    {
        bool UseCompiled
        {
            get;
        }

        IQuery<User> CreateUserById(long userId);

        IQuery<User> CreateUserByName(string userName);

        IQuery<User> CreateUserByApiKey(string apiKey);

        IQuery<int> CreateUserCreatedCountByDates(DateTime from, DateTime to);

        IQuery<int> CreateUserVisitedCountByDates(DateTime from, DateTime to);

        IQuery<ShortUrl> CreateShortUrlById(long shortUrlId);

        IQuery<ShortUrl> CreateShortUrlByHash(string urlHash);

        IQuery<ShortUrl> CreateShortUrlByAlias(string alias);

        IQuery<int> CreateVisitCountByAlias(string alias);

        IQuery<int> CreateShortUrlCountByUserId(long userId);

        IQuery<IEnumerable<ShortUrl>> CreateShortUrlsByUserId(long userId, int start, int max);

        IQuery<int> CreateShortUrlCreatedCountByDates(DateTime from, DateTime to);

        IQuery<int> CreateShortUrlVisitedCountByDates(DateTime from, DateTime to);

        IQuery<bool> CreateBannedIPAddressMatching(string ipAddress);

        IQuery<bool> CreateBannedDomainMatching(string url);

        IQuery<bool> CreateReservedAliasMatching(string alias);

        IQuery<bool> CreateBadWordMatching(string expression);
    }
}