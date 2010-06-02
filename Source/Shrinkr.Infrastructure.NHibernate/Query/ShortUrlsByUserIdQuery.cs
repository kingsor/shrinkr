namespace Shrinkr.Infrastructure.NHibernate.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class ShortUrlsByUserIdQuery : QueryBase<IEnumerable<ShortUrl>>
    {
        private static readonly Expression<Func<Database, long, int, int, IQueryable<ShortUrl>>> expression = (database, id, index, count) => database.Aliases.Where(alias => alias.User.Id == id).OrderByDescending(alias => alias.CreatedAt).Select(alias => alias.ShortUrl).Skip(index).Take(count);
        private static readonly Func<Database, long, int, int, IQueryable<ShortUrl>> plainQuery = expression.Compile();
        
        private readonly long userId;
        private readonly int start;
        private readonly int max;

        public ShortUrlsByUserIdQuery(long userId, int start, int max)
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

            return plainQuery(database, userId, start, max);
        }
    }
}