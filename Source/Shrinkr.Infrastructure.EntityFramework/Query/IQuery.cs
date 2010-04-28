namespace Shrinkr.Infrastructure.EntityFramework
{
    public interface IQuery<out TResult>
    {
        TResult Execute(Database database);
    }
}