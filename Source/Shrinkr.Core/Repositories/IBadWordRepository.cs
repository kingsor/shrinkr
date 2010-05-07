namespace Shrinkr.Repositories
{
    using DomainObjects;

    public interface IBadWordRepository : IRepository<BadWord>
    {
        bool IsMatching(string expression);
    }
}