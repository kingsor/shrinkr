namespace Shrinkr.Repositories
{
    using DomainObjects;

    public interface IVisitRepository : IRepository<Visit>
    {
        int Count(string aliasName);
    }
}