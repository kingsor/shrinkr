namespace Shrinkr.Infrastructure.NHibernate
{
    public interface IQuery<out TResult>
    {
        TResult Execute(Database database);
    }
}