namespace Shrinkr.Infrastructure.NHibernate
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public interface IQueryFactory
    {
        IQuery<User> CreateUserById(long userId);

        IQuery<User> CreateUserByName(string userName);

        IQuery<User> CreateUserByApiKey(string apiKey);

        IQuery<int> CreateUserCreatedCountByDates(DateTime from, DateTime to);

        IQuery<int> CreateUserVisitedCountByDates(DateTime from, DateTime to);

        IQuery<ShortUrl> CreateShortUrlById(long shortUrlId);

        IQuery<ShortUrl> CreateShortUrlByHash(string urlHash);

        IQuery<ShortUrl> CreateShortUrlByAlias(string alias);

        IQuery<IEnumerable<ShortUrl>> CreateShortUrlsByUserId(long userId, int start, int max);

        IQuery<int> CreateShortUrlCountByUserId(long userId);

        IQuery<int> CreateShortUrlCreatedCountByDates(DateTime from, DateTime to);

        IQuery<int> CreateShortUrlVisitedCountByDates(DateTime from, DateTime to);

        IQuery<bool> CreateBadWordMatching(string expression);

        IQuery<bool> CreateBannedDomainMatching(string url);

        IQuery<bool> CreateBannedIPAddressMatching(string ipAddress);

        IQuery<bool> CreateReservedAliasMatching(string alias);

        IQuery<int> CreateVisitCountByAlias(string alias);
    }
}