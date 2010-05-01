namespace Shrinkr.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Data.Objects;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class ShortUrlByAliasQuery : QueryBase<ShortUrl>
    {
        private static readonly Expression<Func<Database, string, ShortUrl>> caseInsensitiveExpression = (database, name) => database.ShortUrls.FirstOrDefault(shortUrl => shortUrl.Aliases.Any(a => a.Name == name));
        private static readonly Func<Database, string, ShortUrl> caseInsensitivePlainQuery = caseInsensitiveExpression.Compile();
        private static readonly Func<Database, string, ShortUrl> caseInsensitiveCompiledQuery = CompiledQuery.Compile(caseInsensitiveExpression);

        private static readonly Expression<Func<Database, string, ShortUrl>> caseSensitiveExpression = (database, name) => database.ShortUrls.FirstOrDefault(shortUrl => shortUrl.Aliases.Any(a => a.Name.Equals(name, StringComparison.Ordinal)));
        private static readonly Func<Database, string, ShortUrl> caseSensitivePlainQuery = caseSensitiveExpression.Compile();
        private static readonly Func<Database, string, ShortUrl> caseSensitiveCompiledQuery = CompiledQuery.Compile(caseSensitiveExpression);

        private readonly bool caseSensitive;
        private readonly string alias;

        public ShortUrlByAliasQuery(bool caseSensitive, bool useCompiled, string alias) : base(useCompiled)
        {
            Check.Argument.IsNotNullOrEmpty(alias, "alias");

            this.caseSensitive = caseSensitive;
            this.alias = alias;
        }

        public override ShortUrl Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled
                       ? (caseSensitive
                              ? caseSensitiveCompiledQuery(database, alias)
                              : caseInsensitiveCompiledQuery(database, alias))
                       : (caseSensitive
                              ? caseSensitivePlainQuery(database, alias)
                              : caseInsensitivePlainQuery(database, alias));
        }
    }
}