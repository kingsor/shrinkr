namespace Shrinkr.Infrastructure.NHibernate.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class BannedIPAddressMatchingQuery : QueryBase<bool>
    {
        private static readonly Expression<Func<Database, string, bool>> expression = (database, ip) => database.BannedIPAddresses.Any(banned => banned.IPAddress == ip);
        private static readonly Func<Database, string, bool> plainQuery = expression.Compile();
        
        private readonly string ipAddress;

        public BannedIPAddressMatchingQuery(string ipAddress)
        {
            Check.Argument.IsNotNullOrEmpty(ipAddress, "ipAddress");

            this.ipAddress = ipAddress;
        }

        public override bool Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return plainQuery(database, ipAddress);
        }
    }
}