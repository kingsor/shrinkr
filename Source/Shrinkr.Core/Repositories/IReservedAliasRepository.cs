namespace Shrinkr.Repositories
{
    using DomainObjects;

    public interface IReservedAliasRepository : IRepository<ReservedAlias>
    {
        bool IsMatching(string aliasName);
    }
}