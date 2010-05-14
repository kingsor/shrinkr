namespace Shrinkr.Infrastructure.NHibernate.Query
{
    public interface IQuery<TResult>
    {
        TResult Execute(Database database);
    }
}
