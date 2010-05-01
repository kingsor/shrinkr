namespace Shrinkr.Repositories
{
    using DomainObjects;

    public interface IBannedIPAddressRepository : IRepository<BannedIPAddress>
    {
        bool IsMatching(string ipAddress);
    }
}