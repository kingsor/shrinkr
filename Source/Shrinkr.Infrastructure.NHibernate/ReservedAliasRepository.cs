namespace Shrinkr.Infrastructure.NHibernate
{
    using DomainObjects;
    using Repositories;

    public class ReservedAliasRepository : RepositoryBase<ReservedAlias>, IReservedAliasRepository
    {
        public ReservedAliasRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }

        public bool IsMatching(string aliasName)
        {
            return QueryFactory.CreateReservedAliasMatching(aliasName).Execute(Database);
        }
    }
}