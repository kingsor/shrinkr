namespace Shrinkr.Infrastructure.NHibernate.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class ShortUrlCreatedCountByDatesQuery : QueryBase<int>
    {
        private static readonly Expression<Func<Database, DateTime, DateTime, int>> expression = (database, start, end) => database.Aliases.Count(alias => alias.CreatedAt >= start && alias.CreatedAt <= end);
        private static readonly Func<Database, DateTime, DateTime, int> plainQuery = expression.Compile();

        private readonly DateTime from;
        private readonly DateTime to;

        public ShortUrlCreatedCountByDatesQuery(DateTime from, DateTime to)
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