namespace Shrinkr.Infrastructure.NHibernate.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class ShortUrlCountByUserIdQuery : QueryBase<int>
    {
        private static readonly Expression<Func<Database, long, int>> expression = (database, id) => database.Aliases.Count(alias => alias.User.Id == id);
        private static readonly Func<Database, long, int> plainQuery = expression.Compile();
        
        private readonly long userId;

        public ShortUrlCountByUserIdQuery(long userId)
        {
            Check.Argument.IsNotNegative(userId, "userId");

            this.userId = userId;
        }

        public override int Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return plainQuery(database, userId);
        }
    }
}