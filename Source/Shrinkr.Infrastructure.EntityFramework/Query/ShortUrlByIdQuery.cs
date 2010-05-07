namespace Shrinkr.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Data.Objects;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class ShortUrlByIdQuery : QueryBase<ShortUrl>
    {
        private static readonly Expression<Func<Database, long, ShortUrl>> expression = (database, id) => database.ShortUrls.SingleOrDefault(shortUrl => shortUrl.Id == id);
        private static readonly Func<Database, long, ShortUrl> plainQuery = expression.Compile();
        private static readonly Func<Database, long, ShortUrl> compiledQuery = CompiledQuery.Compile(expression);

        private readonly long shortUrlId;

        public ShortUrlByIdQuery(bool useCompiled, long shortUrlId) : base(useCompiled)
        {
            Check.Argument.IsNotNegative(shortUrlId, "shortUrlId");

            this.shortUrlId = shortUrlId;
        }

        public override ShortUrl Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled ?
                   compiledQuery(database, shortUrlId) :
                   plainQuery(database, shortUrlId);
        }
    }
}