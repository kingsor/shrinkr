namespace Shrinkr.Repositories
{
    using DomainObjects;

    public interface IBannedDomainRepository : IRepository<BannedDomain>
    {
        bool IsMatching(string url);
    }
}