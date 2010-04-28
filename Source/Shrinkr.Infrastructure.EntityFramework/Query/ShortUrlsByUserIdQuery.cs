namespace Shrinkr.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Collections.Generic;
    using System.Data.Objects;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class ShortUrlsByUserIdQuery : QueryBase<IEnumerable<ShortUrl>>
    {
        private static readonly Expression<Func<Database, long, int, int, IQueryable<ShortUrl>>> expression = (database, id, index, count) => database.Aliases.Where(alias => alias.User.Id == id).OrderByDescending(alias => alias.CreatedAt).Select(alias => alias.ShortUrl).Skip(index).Take(count);
        private static readonly Func<Database, long, int, int, IQueryable<ShortUrl>> plainQuery = expression.Compile();
        private static readonly Func<Database, long, int, int, IQueryable<ShortUrl>> compiledQuery = CompiledQuery.Compile(expression);

        private readonly long userId;
        private readonly int start;
        private readonly int max;

        public ShortUrlsByUserIdQuery(bool useCompiled, long userId, int start, int max) : base(useCompiled)
        {
            Check.Argument.IsNotNegative(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotZeroOrNegative(max, "max");
            this.userId = userId;
            this.start = start;
            this.max = max;
        }

        public override IEnumerable<ShortUrl> Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled ?
                   compiledQuery(database, userId, start, max) :
                   plainQuery(database, userId, start, max);
        }
    }
}