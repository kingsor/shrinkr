namespace Shrinkr.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Data.Objects;
    using System.Linq;
    using System.Linq.Expressions;

    public class BannedIPAddressMatchingQuery : QueryBase<bool>
    {
        private static readonly Expression<Func<Database, string, bool>> expression = (database, ip) => database.BannedIPAddresses.Any(banned => banned.IPAddress == ip);
        private static readonly Func<Database, string, bool> plainQuery = expression.Compile();
        private static readonly Func<Database, string, bool> compiledQuery = CompiledQuery.Compile(expression);

        private readonly string ipAddress;

        public BannedIPAddressMatchingQuery(bool useCompiled, string ipAddress) : base(useCompiled)
        {
            Check.Argument.IsNotNullOrEmpty(ipAddress, "ipAddress");

            this.ipAddress = ipAddress;
        }

        public override bool Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled ?
                                   compiledQuery(database, ipAddress) :
                                                                          plainQuery(database, ipAddress);
        }
    }
}