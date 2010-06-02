namespace Shrinkr.Infrastructure.NHibernate
{
    public interface IQuery<TResult>
    {
        TResult Execute(Database database);
    }
}
