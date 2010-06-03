namespace Shrinkr.Infrastructure.NHibernate
{
    using Query;

    public class QueryFactory : IQueryFactory
    {
        public QueryFactory(bool caseSensitive)
        {
            CaseSensitive = caseSensitive;
        }

        public bool CaseSensitive
        {
            get;
            private set;
        }

        public IQuery<bool> CreateBadWordMatching(string expression)
        {
            return new BadWordMatchingQuery(expression);
        }

        public IQuery<bool> CreateBannedDomainMatching(string url)
        {
            return new BannedDomainMatchingQuery(url);
        }

        public IQuery<bool> CreateBannedIPAddressMatching(string ipAddress)
        {
            return new BannedIPAddressMatchingQuery(ipAddress);
        }

        public IQuery<bool> CreateReservedAliasMatching(string alias)
        {
            return new ReservedAliasMatchingQuery(CaseSensitive, alias);
        }

        public IQuery<int> CreateVisitCountByAlias(string alias)
        {
            return new VisitCountByAliasNameQuery(CaseSensitive, alias);
        }
    }
}
