namespace Shrinkr.Infrastructure.EntityFramework
{
    using DomainObjects;
    using Repositories;

    public class BannedIPAddressRepository : RepositoryBase<BannedIPAddress>, IBannedIPAddressRepository
    {
        public BannedIPAddressRepository(IDatabaseFactory database, IQueryFactory queryFactory) : base(database, queryFactory)
        {
        }

        public bool IsMatching(string ipAddress)
        {
            return QueryFactory.CreateBannedIPAddressMatching(ipAddress).Execute(Database);
        }
    }
}