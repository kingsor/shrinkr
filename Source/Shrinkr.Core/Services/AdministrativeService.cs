namespace Shrinkr.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DataTransferObjects;
    using DomainObjects;
    using Extensions;
    using Infrastructure;
    using Repositories;

    public class AdministrativeService : IAdministrativeService
    {
        private static readonly IList<int> timelines = BuildTimelines();

        private readonly IUserRepository userRepository;
        private readonly IShortUrlRepository shortUrlRepository;
        private readonly IVisitRepository visitRepository;
        private readonly IBannedIPAddressRepository bannedIPAddressRepository;
        private readonly IBannedDomainRepository bannedDomainRepository;
        private readonly IReservedAliasRepository reservedAliasRepository;
        private readonly IBadWordRepository barWordRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUrlResolver urlResolver;
        private readonly ICacheManager cacheManager;

        public AdministrativeService(IUserRepository userRepository, IShortUrlRepository shortUrlRepository, IVisitRepository visitRepository, IBannedIPAddressRepository bannedIPAddressRepository, IBannedDomainRepository bannedDomainRepository, IReservedAliasRepository reservedAliasRepository, IBadWordRepository barWordRepository, IUnitOfWork unitOfWork, IUrlResolver urlResolver, ICacheManager cacheManager)
        {
            Check.Argument.IsNotNull(userRepository, "userRepository");
            Check.Argument.IsNotNull(shortUrlRepository, "shortUrlRepository");
            Check.Argument.IsNotNull(visitRepository, "visitRepository");
            Check.Argument.IsNotNull(bannedIPAddressRepository, "bannedIPAddressRepository");
            Check.Argument.IsNotNull(bannedDomainRepository, "bannedDomainRepository");
            Check.Argument.IsNotNull(reservedAliasRepository, "reservedAliasRepository");
            Check.Argument.IsNotNull(barWordRepository, "barWordRepository");
            Check.Argument.IsNotNull(unitOfWork, "unitOfWork");
            Check.Argument.IsNotNull(urlResolver, "urlResolver");
            Check.Argument.IsNotNull(cacheManager, "cacheManager");

            this.userRepository = userRepository;
            this.shortUrlRepository = shortUrlRepository;
            this.visitRepository = visitRepository;
            this.bannedIPAddressRepository = bannedIPAddressRepository;
            this.bannedDomainRepository = bannedDomainRepository;
            this.reservedAliasRepository = reservedAliasRepository;
            this.barWordRepository = barWordRepository;
            this.unitOfWork = unitOfWork;
            this.urlResolver = urlResolver;
            this.cacheManager = cacheManager;
        }

        public IDictionary<TimeSpan, SystemHealthDTO> GetHealthStatus(DateTime timestamp)
        {
            IDictionary<TimeSpan, SystemHealthDTO> status = new Dictionary<TimeSpan, SystemHealthDTO>();

            foreach (int minutes in timelines)
            {
                DateTime from = timestamp.AddMinutes(-minutes);

                int urlCreated = shortUrlRepository.GetCreatedCount(from, timestamp);
                int urlVisited = shortUrlRepository.GetVisitedCount(from, timestamp);
                int userCreated = userRepository.GetCreatedCount(from, timestamp);
                int userVisited = userRepository.GetVisitedCount(from, timestamp);

                status.Add((timestamp - from), new SystemHealthDTO(urlCreated, urlVisited, userCreated, userVisited));
            }

            return new SortedDictionary<TimeSpan, SystemHealthDTO>(status, new FromMinToMax());
        }

        public UserDTO GetUser(long id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            return new UserDTO(userRepository.GetById(id));
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            return userRepository.All().Select(user => new UserDTO(user));
        }

        public void UpdateUserApiAccess(long id, int dailyLimit)
        {
            User user = userRepository.GetById(id);

            if (dailyLimit > -1)
            {
                user.AllowApiAccess(dailyLimit);
            }
            else
            {
                user.BlockApiAccess();
            }

            unitOfWork.Commit();
        }

        public void LockOrUnlockUser(long id, bool unlock)
        {
            User user = userRepository.GetById(id);
            user.IsLockedOut = !unlock;

            unitOfWork.Commit();
        }

        public void UpdateUserRole(long id, Role role)
        {
            User user = userRepository.GetById(id);
            user.Role = role;

            unitOfWork.Commit();
        }

        public ShortUrlDTO GetShortUrl(string aliasName)
        {
            Check.Argument.IsNotNullOrEmpty(aliasName, "aliasName");

            ShortUrl shortUrl = shortUrlRepository.GetByAliasName(aliasName);
            Alias alias = shortUrl.Aliases.Single(a => a.Name == aliasName);

            return CreateShortUrlDTO(alias);
        }

        public IEnumerable<ShortUrlDTO> GetShortUrls()
        {
            return shortUrlRepository.All().SelectMany(shortUrl => shortUrl.Aliases).Select(CreateShortUrlDTO);
        }

        public void UpdateShortUrlSpamStatus(string aliasName, SpamStatus status)
        {
            ShortUrl shortUrl = shortUrlRepository.GetByAliasName(aliasName);

            shortUrl.SpamStatus = status;

            unitOfWork.Commit();
        }

        public AdministrativeActionResult<BannedIPAddress> CreateBannedIPAddress(string ipAddress)
        {
            AdministrativeActionResult<BannedIPAddress> result = Validation.Validate<AdministrativeActionResult<BannedIPAddress>>(() => string.IsNullOrWhiteSpace(ipAddress), "ipAddress", TextMessages.IpAddressCannotBeBlank)
                                                                          .Or(() => !ipAddress.IsIPAddress(), "ipAddress", TextMessages.IpAddressIsNotInValidFormat)
                                                                          .Or(() => bannedIPAddressRepository.IsMatching(ipAddress), "ipAddress", TextMessages.SpecifiedIpAddressAlreadyExists.FormatWith(ipAddress))
                                                                          .Result();

            if (result.RuleViolations.IsEmpty())
            {
                BannedIPAddress banned = new BannedIPAddress { IPAddress = ipAddress };

                bannedIPAddressRepository.Add(banned);

                unitOfWork.Commit();

                result = new AdministrativeActionResult<BannedIPAddress>(banned);
            }

            return result;
        }

        public void DeleteBannedIPAddress(long id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            bannedIPAddressRepository.Delete(bannedIPAddressRepository.GetById(id));
            unitOfWork.Commit();
        }

        public IEnumerable<BannedIPAddress> GetBannedIPAddresses()
        {
            return bannedIPAddressRepository.All().OrderBy(banned => banned.IPAddress);
        }

        public AdministrativeActionResult<BannedDomain> CreateBannedDomain(string name)
        {
            AdministrativeActionResult<BannedDomain> result = Validation.Validate<AdministrativeActionResult<BannedDomain>>(() => string.IsNullOrWhiteSpace(name), "name", TextMessages.PrefixCannotBeBlank)
                                                                        .Or(() => bannedDomainRepository.IsMatching(name), "prefix", TextMessages.SpecifiedPrefixAlreadyExists.FormatWith(name))
                                                                        .Result();

            if (result.RuleViolations.IsEmpty())
            {
                BannedDomain banned = new BannedDomain { Name = name.ToLower(Culture.Current) };

                bannedDomainRepository.Add(banned);

                unitOfWork.Commit();

                result = new AdministrativeActionResult<BannedDomain>(banned);
            }

            return result;
        }

        public void DeleteBannedDomain(long id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            bannedDomainRepository.Delete(bannedDomainRepository.GetById(id));
            unitOfWork.Commit();
        }

        public IEnumerable<BannedDomain> GetBannedDomains()
        {
            return bannedDomainRepository.All().OrderBy(banned => banned.Name);
        }

        public AdministrativeActionResult<ReservedAlias> CreateReservedAlias(string aliasName)
        {
            AdministrativeActionResult<ReservedAlias> result = Validation.Validate<AdministrativeActionResult<ReservedAlias>>(() => string.IsNullOrWhiteSpace(aliasName), "aliasName", TextMessages.AliasCannotBeBlank)
                                                                       .Or(() => reservedAliasRepository.IsMatching(aliasName), "aliasName", TextMessages.SpecifiedAliasAlreadyExists.FormatWith(aliasName))
                                                                       .Result();

            if (result.RuleViolations.IsEmpty())
            {
                ReservedAlias reserved = new ReservedAlias { Name = aliasName };

                reservedAliasRepository.Add(reserved);

                unitOfWork.Commit();

                result = new AdministrativeActionResult<ReservedAlias>(reserved);
            }

            return result;
        }

        public void DeleteReservedAlias(long id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            reservedAliasRepository.Delete(reservedAliasRepository.GetById(id));
            unitOfWork.Commit();
        }

        public IEnumerable<ReservedAlias> GetReservedAliases()
        {
            return reservedAliasRepository.All().OrderBy(a => a.Name);
        }

        public AdministrativeActionResult<BadWord> CreateBadWord(string expression)
        {
            AdministrativeActionResult<BadWord> result = Validation.Validate<AdministrativeActionResult<BadWord>>(() => string.IsNullOrWhiteSpace(expression), "expression", TextMessages.ExpressionCannotBeBlank)
                                                                   .Or(() => barWordRepository.IsMatching(expression), "expression", TextMessages.SpecifiedExpressionAlreadyExists.FormatWith(expression))
                                                                   .Result();

            if (result.RuleViolations.IsEmpty())
            {
                BadWord badWord = new BadWord { Expression = expression };

                barWordRepository.Add(badWord);

                unitOfWork.Commit();
                cacheManager.Remove(TextMatchingSpamDetector.CacheKey);

                result = new AdministrativeActionResult<BadWord>(badWord);
            }

            return result;
        }

        public void DeleteBadWord(long id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            barWordRepository.Delete(barWordRepository.GetById(id));
            unitOfWork.Commit();

            cacheManager.Remove(TextMatchingSpamDetector.CacheKey);
        }

        public IEnumerable<BadWord> GetBadWords()
        {
            return barWordRepository.All().OrderBy(b => b.Expression);
        }

        private static IList<int> BuildTimelines()
        {
            return new List<int> { 1, 5, 15, 30, 60, (60 * 2), (60 * 6), (60 * 12), (60 * 24), (60 * 24 * 2), (60 * 24 * 7), (60 * 24 * 30), (60 * 24 * 30 * 3), (60 * 24 * 30 * 6) };
        }

        private ShortUrlDTO CreateShortUrlDTO(Alias alias)
        {
            string visitUrl = urlResolver.Absolute(urlResolver.Visit(alias.Name));
            string previewUrl = urlResolver.Absolute(urlResolver.Preview(alias.Name));
            int visits = visitRepository.Count(alias.Name);

            return new ShortUrlDTO(alias, visits, visitUrl, previewUrl);
        }

        private sealed class FromMinToMax : IComparer<TimeSpan>
        {
            public int Compare(TimeSpan x, TimeSpan y)
            {
                return x.CompareTo(y);
            }
        }
    }
}