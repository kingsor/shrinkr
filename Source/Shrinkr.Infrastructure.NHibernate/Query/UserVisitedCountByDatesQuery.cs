namespace Shrinkr.Infrastructure.NHibernate.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class UserVisitedCountByDatesQuery : QueryBase<int>
    {
        private static readonly Expression<Func<Database, DateTime, DateTime, int>> expression = (database, start, end) => database.Users.Count(user => user.LastActivityAt >= start && user.LastActivityAt <= end);
        private static readonly Func<Database, DateTime, DateTime, int> plainQuery = expression.Compile();
        
        private readonly DateTime fromDate;
        private readonly DateTime toDate;

        public UserVisitedCountByDatesQuery(DateTime fromDate, DateTime toDate)
        {
            this.fromDate = fromDate;
            this.toDate = toDate;
        }

        public override int Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return plainQuery(database, fromDate, toDate);
        }
    }
}