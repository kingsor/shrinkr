namespace Shrinkr.Infrastructure.NHibernate.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class BannedDomainMatchingQuery : QueryBase<bool>
    {
        private static readonly Expression<Func<Database, string, bool>> expression = (database, name) => database.BannedDomains.Any(domain => domain.Name == name);
        private static readonly Func<Database, string, bool> plainQuery = expression.Compile();

        private readonly string url;

        public BannedDomainMatchingQuery(string url)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");

            this.url = url;
        }

        public override bool Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            string localUrl = url;

            // add the protocol if missing
            if (!localUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !localUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                localUrl = "http://" + localUrl;
            }

            string domainName = new Uri(localUrl.ToLower(Culture.Current)).Host;
            string[] domainNameParts = domainName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (domainNameParts.Length > 2)
            {
                domainName = string.Join(".", domainNameParts, 1, domainNameParts.Length - 1);
            }

            return plainQuery(database, domainName);
        }
    }
}