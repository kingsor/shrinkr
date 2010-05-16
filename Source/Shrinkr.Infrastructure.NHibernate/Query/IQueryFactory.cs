namespace Shrinkr.Infrastructure.NHibernate
{
    using Query;

    public interface IQueryFactory
    {
        IQuery<bool> CreateBadWordMatching(string expression);

        IQuery<bool> CreateBannedDomainMatching(string url);

        IQuery<bool> CreateBannedIPAddressMatching(string ipAddress);

        IQuery<bool> CreateReservedAliasMatching(string alias);

        IQuery<int> CreateVisitCountByAlias(string alias);
    }
}
