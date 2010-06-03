namespace Shrinkr.Infrastructure.NHibernate.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class ShortUrlVisitedCountByDatesQuery : QueryBase<int>
    {
        private static readonly Expression<Func<Database, DateTime, DateTime, int>> expression = (database, start, end) => database.Visits.Count(visit => visit.CreatedAt >= start && visit.CreatedAt <= end);
        private static readonly Func<Database, DateTime, DateTime, int> plainQuery = expression.Compile();
        
        private readonly DateTime from;
        private readonly DateTime to;

        public ShortUrlVisitedCountByDatesQuery(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        public override int Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return plainQuery(database, from, to);
        }
    }
}