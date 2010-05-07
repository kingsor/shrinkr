namespace Shrinkr.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Data.Objects;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class ShortUrlByHashQuery : QueryBase<ShortUrl>
    {
        private static readonly Expression<Func<Database, string, ShortUrl>> expression = (database, hash) => database.ShortUrls.FirstOrDefault(shortUrl => shortUrl.Hash == hash);
        private static readonly Func<Database, string, ShortUrl> plainQuery = expression.Compile();
        private static readonly Func<Database, string, ShortUrl> compiledQuery = CompiledQuery.Compile(expression);

        private readonly string urlHash;

        public ShortUrlByHashQuery(bool useCompiled, string urlHash) : base(useCompiled)
        {
            Check.Argument.IsNotNullOrEmpty(urlHash, "urlHash");

            this.urlHash = urlHash;
        }

        public override ShortUrl Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled ?
                   compiledQuery(database, urlHash) :
                   plainQuery(database, urlHash);
        }
    }
}