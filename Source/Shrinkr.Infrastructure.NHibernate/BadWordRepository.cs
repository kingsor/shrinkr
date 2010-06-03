namespace Shrinkr.Infrastructure.NHibernate
{
    using DomainObjects;
    using Repositories;

    public class BadWordRepository : RepositoryBase<BadWord>, IBadWordRepository
    {
        public BadWordRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }

        public bool IsMatching(string expression)
        {
            return QueryFactory.CreateBadWordMatching(expression).Execute(Database);
        }
    }
}