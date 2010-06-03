namespace Shrinkr.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}