namespace Shrinkr.Services
{
    using System;
    using System.Collections.Generic;

    using DataTransferObjects;
    using DomainObjects;

    public interface IAdministrativeService
    {
        IDictionary<TimeSpan, SystemHealthDTO> GetHealthStatus(DateTime timestamp);

        UserDTO GetUser(long id);

        IEnumerable<UserDTO> GetUsers();

        void UpdateUserApiAccess(long id, int dailyLimit);

        void LockOrUnlockUser(long id, bool unlock);

        void UpdateUserRole(long id, Role role);

        ShortUrlDTO GetShortUrl(string aliasName);

        IEnumerable<ShortUrlDTO> GetShortUrls();

        void UpdateShortUrlSpamStatus(string aliasName, SpamStatus status);

        AdministrativeActionResult<BannedIPAddress> CreateBannedIPAddress(string ipAddress);

        void DeleteBannedIPAddress(long id);

        IEnumerable<BannedIPAddress> GetBannedIPAddresses();

        AdministrativeActionResult<BannedDomain> CreateBannedDomain(string name);

        void DeleteBannedDomain(long id);

        IEnumerable<BannedDomain> GetBannedDomains();

        AdministrativeActionResult<ReservedAlias> CreateReservedAlias(string aliasName);

        void DeleteReservedAlias(long id);

        IEnumerable<ReservedAlias> GetReservedAliases();

        AdministrativeActionResult<BadWord> CreateBadWord(string expression);

        void DeleteBadWord(long id);

        IEnumerable<BadWord> GetBadWords();
    }
}