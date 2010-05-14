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
    }
}
