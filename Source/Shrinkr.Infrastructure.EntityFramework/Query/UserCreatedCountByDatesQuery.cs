namespace Shrinkr.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Data.Objects;
    using System.Linq;
    using System.Linq.Expressions;

    public class UserCreatedCountByDatesQuery : QueryBase<int>
    {
        private static readonly Expression<Func<Database, DateTime, DateTime, int>> expression = (database, start, end) => database.Users.Count(user => user.CreatedAt >= start && user.CreatedAt <= end);
        private static readonly Func<Database, DateTime, DateTime, int> plainQuery = expression.Compile();
        private static readonly Func<Database, DateTime, DateTime, int> compiledQuery = CompiledQuery.Compile(expression);

        private readonly DateTime fromDate;
        private readonly DateTime toDate;

        public UserCreatedCountByDatesQuery(bool useCompiled, DateTime fromDate, DateTime toDate) : base(useCompiled)
        {
            this.fromDate = fromDate;
            this.toDate = toDate;
        }

        public override int Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled ?
                   compiledQuery(database, fromDate, toDate) :
                   plainQuery(database, fromDate, toDate);
        }
    }
}