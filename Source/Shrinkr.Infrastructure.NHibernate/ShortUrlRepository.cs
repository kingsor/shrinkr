namespace Shrinkr.Infrastructure.NHibernate
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;
    using Repositories;

    public class ShortUrlRepository : RepositoryBase<ShortUrl>, IShortUrlRepository
    {
        public ShortUrlRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }

        public override ShortUrl GetById(long id)
        {
            IQuery<ShortUrl> query = QueryFactory.CreateShortUrlById(id);

            return query.Execute(Database);
        }

        public ShortUrl GetByHash(string hash)
        {
            IQuery<ShortUrl> query = QueryFactory.CreateShortUrlByHash(hash);

            return query.Execute(Database);
        }

        public ShortUrl GetByAliasName(string aliasName)
        {
            IQuery<ShortUrl> query = QueryFactory.CreateShortUrlByAlias(aliasName);

            return query.Execute(Database);
        }

        public PagedResult<ShortUrl> FindByUserId(long userId, int start, int max)
        {
            IQuery<int> countQuery = QueryFactory.CreateShortUrlCountByUserId(userId);
            IQuery<IEnumerable<ShortUrl>> pagedQuery = QueryFactory.CreateShortUrlsByUserId(userId, start, max);

            int total = countQuery.Execute(Database);
            IEnumerable<ShortUrl> result = pagedQuery.Execute(Database);

            return new PagedResult<ShortUrl>(result, total);
        }

        public int GetCreatedCount(DateTime fromDate, DateTime toDate)
        {
            IQuery<int> query = QueryFactory.CreateShortUrlCreatedCountByDates(fromDate, toDate);

            return query.Execute(Database);
        }

        public int GetVisitedCount(DateTime fromDate, DateTime toDate)
        {
            IQuery<int> query = QueryFactory.CreateShortUrlVisitedCountByDates(fromDate, toDate);

            return query.Execute(Database);
        }
    }
}