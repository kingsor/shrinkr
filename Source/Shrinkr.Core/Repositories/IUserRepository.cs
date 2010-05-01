namespace Shrinkr.Repositories
{
    using System;

    using DomainObjects;

    public interface IUserRepository : IRepository<User>
    {
        User GetByName(string name);

        User GetByApiKey(string apiKey);

        int GetCreatedCount(DateTime fromDate, DateTime toDate);

        int GetVisitedCount(DateTime fromDate, DateTime toDate);
    }
}