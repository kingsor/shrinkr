namespace Shrinkr.Infrastructure.EntityFramework
{
    using DomainObjects;
    using Repositories;

    public class VisitRepository : RepositoryBase<Visit>, IVisitRepository
    {
        public VisitRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }

        public int Count(string aliasName)
        {
            IQuery<int> query = QueryFactory.CreateVisitCountByAlias(aliasName);

            return query.Execute(Database);
        }
    }
}