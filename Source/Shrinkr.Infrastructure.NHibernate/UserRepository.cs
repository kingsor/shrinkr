namespace Shrinkr.Infrastructure.NHibernate    
{
    using System;

    using DomainObjects;
    using Repositories;

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }

        public override User GetById(long id)
        {
            IQuery<User> query = QueryFactory.CreateUserById(id);

            return query.Execute(Database);
        }

        public User GetByName(string name)
        {
            IQuery<User> query = QueryFactory.CreateUserByName(name);

            return query.Execute(Database);
        }

        public User GetByApiKey(string apiKey)
        {
            IQuery<User> query = QueryFactory.CreateUserByApiKey(apiKey);

            return query.Execute(Database);
        }

        public int GetCreatedCount(DateTime fromDate, DateTime toDate)
        {
            IQuery<int> query = QueryFactory.CreateUserCreatedCountByDates(fromDate, toDate);

            return query.Execute(Database);
        }

        public int GetVisitedCount(DateTime fromDate, DateTime toDate)
        {
            IQuery<int> query = QueryFactory.CreateUserVisitedCountByDates(fromDate, toDate);

            return query.Execute(Database);
        }
    }
}