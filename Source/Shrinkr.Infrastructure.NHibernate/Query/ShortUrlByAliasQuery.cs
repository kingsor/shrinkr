namespace Shrinkr.Infrastructure.NHibernate.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class ShortUrlByAliasQuery : QueryBase<ShortUrl>
    {
        private static readonly Expression<Func<Database, string, ShortUrl>> shortUrlExpression = (database, name) => database.ShortUrls.FirstOrDefault(shortUrl => shortUrl.Aliases.Any(a => a.Name == name));
        private static readonly Func<Database, string, ShortUrl> shortUrlPlainQuery = shortUrlExpression.Compile();

        private readonly bool caseSensitive;
        private readonly string alias;

        public ShortUrlByAliasQuery(bool caseSensitive, string alias) 
        {
            Check.Argument.IsNotNullOrEmpty(alias, "alias");

            this.caseSensitive = caseSensitive;
            this.alias = alias;
        }

        public override ShortUrl Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            ShortUrl shortUrl = shortUrlPlainQuery(database, alias);

            if (caseSensitive && shortUrl != null)
            {
                return shortUrl.Aliases.Any(a => a.Name.Equals(alias, StringComparison.Ordinal))
                           ? shortUrl
                           : null;
            }

            return shortUrl;
        }
    }
}