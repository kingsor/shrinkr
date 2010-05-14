namespace Shrinkr.Infrastructure.NHibernate
{
    using Query;

    public interface IQueryFactory
    {
        IQuery<bool> CreateBadWordMatching(string expression);
    }
}
