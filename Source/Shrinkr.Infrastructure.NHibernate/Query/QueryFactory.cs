namespace Shrinkr.Infrastructure.NHibernate
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;
    using Query;

    public class QueryFactory : IQueryFactory
    {
        public QueryFactory(bool caseSensitive)
        {
            CaseSensitive = caseSensitive;
        }

        public bool CaseSensitive
        {
            get;
            private set;
        }

        public IQuery<User> CreateUserById(long userId)
        {
            return new UserByIdQuery(userId);
        }

        public IQuery<User> CreateUserByName(string userName)
        {
            return new UserByNameQuery(userName);
        }

        public IQuery<User> CreateUserByApiKey(string apiKey)
        {
            return new UserByApiKeyQuery(apiKey);
        }

        public IQuery<int> CreateUserCreatedCountByDates(DateTime from, DateTime to)
        {
            return new UserCreatedCountByDatesQuery(from, to);
        }

        public IQuery<int> CreateUserVisitedCountByDates(DateTime from, DateTime to)
        {
            return new UserVisitedCountByDatesQuery(from, to);
        }

        public IQuery<ShortUrl> CreateShortUrlById(long shortUrlId)
        {
            return new ShortUrlByIdQuery(shortUrlId);
        }

        public IQuery<ShortUrl> CreateShortUrlByHash(string urlHash)
        {
            return new ShortUrlByHashQuery(urlHash);
        }

        public IQuery<ShortUrl> CreateShortUrlByAlias(string alias)
        {
            return new ShortUrlByAliasQuery(CaseSensitive, alias);
        }

        public IQuery<int> CreateShortUrlCountByUserId(long userId)
        {
            return new ShortUrlCountByUserIdQuery(userId);
        }

        public IQuery<IEnumerable<ShortUrl>> CreateShortUrlsByUserId(long userId, int start, int max)
        {
            return new ShortUrlsByUserIdQuery(userId, start, max);
        }

        public IQuery<int> CreateShortUrlCreatedCountByDates(DateTime from, DateTime to)
        {
            return new ShortUrlCreatedCountByDatesQuery(from, to);
        }

        public IQuery<int> CreateShortUrlVisitedCountByDates(DateTime from, DateTime to)
        {
            return new ShortUrlVisitedCountByDatesQuery(from, to);
        }

        public IQuery<bool> CreateBadWordMatching(string expression)
        {
            return new BadWordMatchingQuery(expression);
        }

        public IQuery<bool> CreateBannedDomainMatching(string url)
        {
            return new BannedDomainMatchingQuery(url);
        }

        public IQuery<bool> CreateBannedIPAddressMatching(string ipAddress)
        {
            return new BannedIPAddressMatchingQuery(ipAddress);
        }

        public IQuery<bool> CreateReservedAliasMatching(string alias)
        {
            return new ReservedAliasMatchingQuery(CaseSensitive, alias);
        }

        public IQuery<int> CreateVisitCountByAlias(string alias)
        {
            return new VisitCountByAliasNameQuery(CaseSensitive, alias);
        }
    }
}
