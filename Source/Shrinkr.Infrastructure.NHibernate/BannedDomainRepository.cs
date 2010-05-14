namespace Shrinkr.Infrastructure.NHibernate
{
    using DomainObjects;
    using Repositories;

    public class BannedDomainRepository : RepositoryBase<BannedDomain>, IBannedDomainRepository
    {
        public BannedDomainRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) 
            : base(databaseFactory, queryFactory)
        {
        }

        public bool IsMatching(string url)
        {
            return QueryFactory.CreateBannedDomainMatching(url).Execute(Database);
        }
    }
}