namespace Shrinkr.Infrastructure.EntityFramework
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;
    using Query;

    public class QueryFactory : IQueryFactory
    {
        public QueryFactory(bool caseSensitive, bool useCompiled)
        {
            CaseSensitive = caseSensitive;
            UseCompiled = useCompiled;
        }

        public bool UseCompiled
        {
            get;
            private set;
        }

        public bool CaseSensitive
        {
            get;
            private set;
        }

        public IQuery<User> CreateUserById(long userId)
        {
            return new UserByIdQuery(UseCompiled, userId);
        }

        public IQuery<User> CreateUserByName(string userName)
        {
            return new UserByNameQuery(UseCompiled, userName);
        }

        public IQuery<User> CreateUserByApiKey(string apiKey)
        {
            return new UserByApiKeyQuery(UseCompiled, apiKey);
        }

        public IQuery<int> CreateUserCreatedCountByDates(DateTime from, DateTime to)
        {
            return new UserCreatedCountByDatesQuery(UseCompiled, from, to);
        }

        public IQuery<int> CreateUserVisitedCountByDates(DateTime from, DateTime to)
        {
            return new UserVisitedCountByDatesQuery(UseCompiled, from, to);
        }

        public IQuery<ShortUrl> CreateShortUrlById(long shortUrlId)
        {
            return new ShortUrlByIdQuery(UseCompiled, shortUrlId);
        }

        public IQuery<ShortUrl> CreateShortUrlByHash(string urlHash)
        {
            return new ShortUrlByHashQuery(UseCompiled, urlHash);
        }

        public IQuery<ShortUrl> CreateShortUrlByAlias(string alias)
        {
            return new ShortUrlByAliasQuery(CaseSensitive, UseCompiled, alias);
        }

        public IQuery<int> CreateVisitCountByAlias(string alias)
        {
            return new VisitCountByAliasNameQuery(CaseSensitive, UseCompiled, alias);
        }

        public IQuery<int> CreateShortUrlCountByUserId(long userId)
        {
            return new ShortUrlCountByUserIdQuery(UseCompiled, userId);
        }

        public IQuery<IEnumerable<ShortUrl>> CreateShortUrlsByUserId(long userId, int start, int max)
        {
            return new ShortUrlsByUserIdQuery(UseCompiled, userId, start, max);
        }

        public IQuery<int> CreateShortUrlCreatedCountByDates(DateTime from, DateTime to)
        {
            return new ShortUrlCreatedCountByDatesQuery(UseCompiled, from, to);
        }

        public IQuery<int> CreateShortUrlVisitedCountByDates(DateTime from, DateTime to)
        {
            return new ShortUrlVisitedCountByDatesQuery(UseCompiled, from, to);
        }

        public IQuery<bool> CreateBannedIPAddressMatching(string ipAddress)
        {
            return new BannedIPAddressMatchingQuery(UseCompiled, ipAddress);
        }

        public IQuery<bool> CreateBannedDomainMatching(string url)
        {
            return new BannedDomainMatchingQuery(UseCompiled, url);
        }

        public IQuery<bool> CreateReservedAliasMatching(string alias)
        {
            return new ReservedAliasMatchingQuery(CaseSensitive, UseCompiled, alias);
        }

        public IQuery<bool> CreateBadWordMatching(string expression)
        {
            return new BadWordMatchingQuery(UseCompiled, expression);
        }
    }
}