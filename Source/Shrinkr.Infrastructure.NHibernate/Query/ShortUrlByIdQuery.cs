namespace Shrinkr.Infrastructure.NHibernate.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class ShortUrlByIdQuery : QueryBase<ShortUrl>
    {
        private static readonly Expression<Func<Database, long, ShortUrl>> expression = (database, id) => database.ShortUrls.SingleOrDefault(shortUrl => shortUrl.Id == id);
        private static readonly Func<Database, long, ShortUrl> plainQuery = expression.Compile();

        private readonly long shortUrlId;

        public ShortUrlByIdQuery(long shortUrlId)
        {
            Check.Argument.IsNotNegative(shortUrlId, "shortUrlId");

            this.shortUrlId = shortUrlId;
        }

        public override ShortUrl Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return plainQuery(database, shortUrlId);
        }
    }
}