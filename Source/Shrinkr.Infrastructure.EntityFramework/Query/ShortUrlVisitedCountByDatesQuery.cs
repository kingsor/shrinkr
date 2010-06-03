namespace Shrinkr.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Data.Objects;
    using System.Linq;
    using System.Linq.Expressions;

    public class ShortUrlVisitedCountByDatesQuery : QueryBase<int>
    {
        private static readonly Expression<Func<Database, DateTime, DateTime, int>> expression = (database, start, end) => database.Visits.Count(visit => visit.CreatedAt >= start && visit.CreatedAt <= end);
        private static readonly Func<Database, DateTime, DateTime, int> plainQuery = expression.Compile();
        private static readonly Func<Database, DateTime, DateTime, int> compiledQuery = CompiledQuery.Compile(expression);

        private readonly DateTime from;
        private readonly DateTime to;

        public ShortUrlVisitedCountByDatesQuery(bool useCompiled, DateTime from, DateTime to) : base(useCompiled)
        {
            this.from = from;
            this.to = to;
        }

        public override int Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled ?
                   compiledQuery(database, from, to) :
                   plainQuery(database, from, to);
        }
    }
}