namespace Shrinkr.Infrastructure.NHibernate.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class UserByIdQuery : QueryBase<User>
    {
        private static readonly Expression<Func<Database, long, User>> expression = (database, id) => database.Users.SingleOrDefault(user => user.Id == id);
        private static readonly Func<Database, long, User> plainQuery = expression.Compile();
        
        private readonly long userId;

        public UserByIdQuery(long userId)
        {
            Check.Argument.IsNotNegative(userId, "userId");

            this.userId = userId;
        }

        public override User Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return plainQuery(database, userId);
        }
    }
}