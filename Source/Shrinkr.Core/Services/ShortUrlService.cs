namespace Shrinkr.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using DataTransferObjects;
    using DomainObjects;
    using Extensions;
    using Infrastructure;
    using Repositories;

    public class ShortUrlService : IShortUrlService
    {
        private readonly IUserRepository userRepository;
        private readonly IShortUrlRepository shortUrlRepository;
        private readonly IVisitRepository visitRepository;
        private readonly IBannedDomainRepository bannedDomainRepository;
        private readonly IReservedAliasRepository reservedAliasRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IExternalContentService externalContentService;
        private readonly IThumbnail thumbnail;
        private readonly IBaseX baseX;
        private readonly IUrlResolver urlResolver;
        private readonly IEventAggregator eventAggregator;
        private readonly IEnumerable<ISpamDetector> spamDetectors;

        public ShortUrlService(IUserRepository userRepository, IShortUrlRepository shortUrlRepository, IVisitRepository visitRepository, IBannedDomainRepository bannedDomainRepository, IReservedAliasRepository reservedAliasRepository, IUnitOfWork unitOfWork, IExternalContentService externalContentService, IThumbnail thumbnail, IBaseX baseX, IUrlResolver urlResolver, IEventAggregator eventAggregator, ISpamDetector[] spamDetectors)
        {
            Check.Argument.IsNotNull(userRepository, "userRepository");
            Check.Argument.IsNotNull(shortUrlRepository, "shortUrlRepository");
            Check.Argument.IsNotNull(visitRepository, "visitRepository");
            Check.Argument.IsNotNull(bannedDomainRepository, "bannedDomainRepository");
            Check.Argument.IsNotNull(reservedAliasRepository, "reservedAliasRepository");
            Check.Argument.IsNotNull(unitOfWork, "unitOfWork");
            Check.Argument.IsNotNull(externalContentService, "externalContentService");
            Check.Argument.IsNotNull(thumbnail, "thumbnail");
            Check.Argument.IsNotNull(baseX, "baseX");
            Check.Argument.IsNotNull(urlResolver, "urlResolver");
            Check.Argument.IsNotNull(eventAggregator, "eventAggregator");
            Check.Argument.IsNotNull(spamDetectors, "spamDetectors");

            this.userRepository = userRepository;
            this.shortUrlRepository = shortUrlRepository;
            this.visitRepository = visitRepository;
            this.bannedDomainRepository = bannedDomainRepository;
            this.reservedAliasRepository = reservedAliasRepository;
            this.unitOfWork = unitOfWork;
            this.externalContentService = externalContentService;
            this.thumbnail = thumbnail;
            this.baseX = baseX;
            this.urlResolver = urlResolver;
            this.eventAggregator = eventAggregator;
            this.spamDetectors = spamDetectors;
        }

        public ShortUrlResult CreateWithUserName(string url, string aliasName, string ipAddress, string userName)
        {
            return Create(url, aliasName, ipAddress, userName, null, true);
        }

        public ShortUrlResult CreateWithApiKey(string url, string aliasName, string ipAddress, string apiKey)
        {
            return Create(url, aliasName, ipAddress, null, apiKey, false);
        }

        public ShortUrlResult GetByAlias(string aliasName)
        {
            ShortUrlResult result = Validation.Validate<ShortUrlResult>(() => string.IsNullOrWhiteSpace(aliasName), "alias", TextMessages.AliasCannotBeBlank)
                                              .Or(() => !baseX.IsValid(aliasName), "alias", TextMessages.AliasIsNotValidAliasCanOnlyContainAlphanumericCharacters)
                                              .Result();

            if (result.RuleViolations.IsEmpty())
            {
                Alias alias = GetAlias(aliasName, out result);

                if (result.RuleViolations.IsEmpty())
                {
                    result = new ShortUrlResult(CreateShortUrlDTO(alias));
                }
            }

            return result;
        }

        public VisitResult Visit(string aliasName, string ipAddress, string browser, string referrer)
        {
            VisitResult result = Validation.Validate<VisitResult>(() => string.IsNullOrWhiteSpace(aliasName), "alias", TextMessages.AliasCannotBeBlank)
                                           .Or(() => !baseX.IsValid(aliasName), "alias", TextMessages.AliasIsNotValidAliasCanOnlyContainAlphanumericCharacters)
                                           .And(() => string.IsNullOrWhiteSpace(ipAddress), "ipAddress", TextMessages.IpAddressCannotBeBlank)
                                           .Or(() => !ipAddress.IsIPAddress(), "ipAddress", TextMessages.IpAddressIsNotInValidFormat)
                                           .Result();

            if (result.RuleViolations.IsEmpty())
            {
                Alias alias = GetAlias(aliasName, out result);

                if (result.RuleViolations.IsEmpty())
                {
                    string referrerDomain = null;
                    Uri referrerUri;

                    if (Uri.TryCreate(referrer, UriKind.Absolute, out referrerUri))
                    {
                        referrerDomain = referrerUri.Host;
                    }

                    Visit visit = new Visit { IPAddress = ipAddress, Browser = browser, Referrer = new Referrer { Domain = referrerDomain, Url = referrer }, Alias = alias };

                    visitRepository.Add(visit);

                    unitOfWork.Commit();

                    result = new VisitResult(new VisitDTO(visit));

                    eventAggregator.GetEvent<ShortUrlVisitedEvent>().Publish(new EventArgs<Visit>(visit));
                }
            }

            return result;
        }

        public ShortUrlListResult FindByUser(string userName, int start, int max)
        {
            ShortUrlListResult result = Validation.Validate<ShortUrlListResult>(() => string.IsNullOrWhiteSpace(userName), "userName", TextMessages.UserNameCannotBeBlank)
                                             .And(() => start < 0, "start", TextMessages.StartIndexCannotBeNegative)
                                             .And(() => max <= 0, "max", TextMessages.MaximumNumberOfUrlsMustBePositive)
                                             .Result();

            if (result.RuleViolations.IsEmpty())
            {
                User user;

                // Ensure user is not locked out.
                result = ValidateUserName<ShortUrlListResult>(userName, out user);

                if (result.RuleViolations.IsEmpty())
                {
                    PagedResult<ShortUrl> pagedResult = shortUrlRepository.FindByUserId(user.Id, start, max);

                    // Convert it to DTOs
                    IEnumerable<ShortUrlDTO> dtos = pagedResult.Result
                                                               .SelectMany(shortUrl => shortUrl.Aliases.Where(alias => (alias.User != null) && alias.User.Id == user.Id))
                                                               .Distinct()
                                                               .Select(CreateShortUrlDTO);

                    result = new ShortUrlListResult(new PagedResult<ShortUrlDTO>(dtos, pagedResult.Total));
                }
            }

            return result;
        }

        private ShortUrlResult Create(string url, string aliasName, string ipAddress, string userName, string apiKey, bool useUserName)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                // add the protocol if missing
                if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                    !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    url = "http://" + url;
                }
            }

            ShortUrlResult result = Validation.Validate<ShortUrlResult>(() => string.IsNullOrWhiteSpace(url), "url", TextMessages.UrlCannotBeBlank)
                                              .Or(() => !url.IsWebUrl(), "url", TextMessages.UrlIsNotInValidFormat)
                                              .Or(() => url.StartsWith(urlResolver.ApplicationRoot, StringComparison.OrdinalIgnoreCase), "url", TextMessages.CannotShrinkUrlForItsOwnDomain)
                                              .Or(() => bannedDomainRepository.IsMatching(url), "url", TextMessages.CannotShrinkUrlWhichMatchesWithOurBannedDomains)
                                              .And(() => !string.IsNullOrWhiteSpace(aliasName) && !baseX.IsValid(aliasName), "alias", TextMessages.AliasIsNotValidAliasCanOnlyContainAlphanumericCharacters)
                                              .Or(() => !string.IsNullOrWhiteSpace(aliasName) && reservedAliasRepository.IsMatching(aliasName), "alias", TextMessages.SpecifiedAliasMatchesWithOurReservedAlias.FormatWith(aliasName))
                                              .And(() => string.IsNullOrWhiteSpace(ipAddress), "ipAddress", TextMessages.IpAddressCannotBeBlank)
                                              .Or(() => !ipAddress.IsIPAddress(), "ipAddress", TextMessages.IpAddressIsNotInValidFormat)
                                              .Result();

            if (result.RuleViolations.IsEmpty())
            {
                User user;

                result = useUserName ?
                         ValidateUserName<ShortUrlResult>(userName, out user) :
                         ValidateApiKey<ShortUrlResult>(apiKey, out user);

                if (result.RuleViolations.IsEmpty())
                {
                    string hash = url.ToLower(Culture.Invariant).Hash();
                    ShortUrl shortUrl = shortUrlRepository.GetByHash(hash);

                    if (shortUrl == null)
                    {
                        // Url does not exist, create it, if url is not valid then we
                        // will have the error in the rule violations.
                        ExternalContent externalContent = GetContent(url, out result);

                        if (result.RuleViolations.IsEmpty())
                        {
                            shortUrl = new ShortUrl { Title = externalContent.Title, Url = url, Domain = new Uri(url, UriKind.Absolute).Host.ToLower(Culture.Current), Hash = hash };

                            thumbnail.Capture(url);

                            shortUrlRepository.Add(shortUrl);
                        }
                    }

                    if (result.RuleViolations.IsEmpty() && (shortUrl != null))
                    {
                        if (string.IsNullOrWhiteSpace(aliasName))
                        {
                            if (shortUrl.Id == 0)
                            {
                                result = CreateWithoutAlias(shortUrl, ipAddress, user, !useUserName);
                            }
                            else
                            {
                                if (user == null)
                                {
                                    // Check for existing alias which does not have an associated user;
                                    Alias alias = shortUrl.Aliases.FirstOrDefault(a => a.User == null);

                                    result = alias != null ?
                                             new ShortUrlResult(CreateShortUrlDTO(alias)) :
                                             CreateWithoutAlias(shortUrl, ipAddress, user, !useUserName);
                                }
                                else
                                {
                                    Alias alias = shortUrl.Aliases.FirstOrDefault(a => (a.User != null) && (a.User.Id == user.Id));

                                    result = alias != null ?
                                             CreateWithAlias(shortUrl, alias.Name, ipAddress, user, !useUserName) :
                                             CreateWithoutAlias(shortUrl, ipAddress, user, !useUserName);
                                }
                            }
                        }
                        else
                        {
                            result = CreateWithAlias(shortUrl, aliasName, ipAddress, user, !useUserName);
                        }

                        if (result.RuleViolations.IsEmpty())
                        {
                            unitOfWork.Commit();
                        }
                    }
                }
            }

            return result;
        }

        private T ValidateUserName<T>(string userName, out User user) where T : ServiceResultBase, new()
        {
            T result = new T();
            user = null;

            // We have to check whether the user is specified.
            // If user is specified we have to ensure that the
            // user is not currently locked out.
            if (!string.IsNullOrWhiteSpace(userName))
            {
                // We have to check whether the user is specified.
                // If user is specified we have to ensure that the
                // user is not currently locked out.
                User tempUser = userRepository.GetByName(userName);
                user = tempUser;

                result = Validation.Validate<T>(() => tempUser == null, "userName", TextMessages.UserDoesNotExist.FormatWith(userName))
                                   .Or(() => tempUser.IsLockedOut, "userName", TextMessages.UserIsCurrentlyLockedOut.FormatWith(userName))
                                   .Result();
            }

            return result;
        }

        private T ValidateApiKey<T>(string apiKey, out User user) where T : ServiceResultBase, new()
        {
            T result = Validation.Validate<T>(() => string.IsNullOrWhiteSpace(apiKey), "apiKey", TextMessages.ApiKeyCannotBeBlank).Result();
            user = null;

            if (result.RuleViolations.IsNullOrEmpty())
            {
                User tempUser = userRepository.GetByApiKey(apiKey);
                user = tempUser;

                result = Validation.Validate<T>(() => tempUser == null, "apiKey", TextMessages.InvalidApiKey.FormatWith(apiKey))
                                   .Or(() => tempUser.IsLockedOut, "apiKey", TextMessages.UserWithSpecifiedApiKeyIsLockedOut.FormatWith(apiKey))
                                   .Or(() => !tempUser.CanAccessApi, "apiKey", TextMessages.UserIsNotAllowedToAccessWithSpecifiedApiKey.FormatWith(apiKey))
                                   .Or(() => tempUser.HasExceededDailyLimit(), "apiKey", TextMessages.UserHasAlreadyReachedYourDailyLimit)
                                   .Result();
            }

            return result;
        }

        private Alias GetAlias<T>(string aliasName, out T result) where T : ServiceResultBase, new()
        {
            result = new T();
            Alias alias = null;

            ShortUrl shortUrl = shortUrlRepository.GetByAliasName(aliasName);
            result = Validation.Validate<T>(() => shortUrl == null, "alias", TextMessages.UrlWithTheSpecifiedAliasDoesNotExist.FormatWith(aliasName)).Result();

            if (result.RuleViolations.IsEmpty())
            {
                // TODO: Consider casing
                alias = shortUrl.Aliases.Single(a => a.Name.Equals(aliasName, StringComparison.OrdinalIgnoreCase));
            }

            return alias;
        }

        private ExternalContent GetContent(string url, out ShortUrlResult result)
        {
            result = new ShortUrlResult();
            ExternalContent externalContent = null;

            try
            {
                externalContent = externalContentService.Retrieve(url);
            }
            catch (WebException e)
            {
                result = new ShortUrlResult(new[] { new RuleViolation("url", e.Message) });
            }

            return externalContent;
        }

        private ShortUrlResult CreateWithoutAlias(ShortUrl shortUrl, string ipAddress, User user, bool withApi)
        {
            Alias alias = new Alias { IPAddress = ipAddress, CreatedByApi = withApi, ShortUrl = shortUrl, User = user };

            // Associate with url
            shortUrl.Aliases.Add(alias);

            if (user != null)
            {
                user.Aliases.Add(alias);
            }

            // We have to commit to generate the alias id
            unitOfWork.Commit();

            long id = alias.Id;

            // We also need to ensure that the generated alias does not exists.
            // it might happen if the same alias is specified previously by another user
            // we also have to ensure that the generated alias does match with the reserved alias
            while (true)
            {
                string aliasName = baseX.Encode(id);

                if ((shortUrlRepository.GetByAliasName(aliasName) == null) && !reservedAliasRepository.IsMatching(aliasName))
                {
                    alias.Name = aliasName;
                    break;
                }

                // Alias exists, So try next free alias
                id += 1;
            }

            OnCreate(alias);

            return new ShortUrlResult(CreateShortUrlDTO(alias));
        }

        private ShortUrlResult CreateWithAlias(ShortUrl shortUrl, string aliasName, string ipAddress, User user, bool withApi)
        {
            // Alias is specified
            ShortUrlResult result = new ShortUrlResult();
            ShortUrl urlWithSpecifiedAlias = shortUrlRepository.GetByAliasName(aliasName);

            // Url exists, we will return the alias if it is requested by the 
            // same user for the same url with the same alias
            if (urlWithSpecifiedAlias != null)
            {
                Alias alias = ((user != null) && (shortUrl.Id == urlWithSpecifiedAlias.Id)) ?
                               urlWithSpecifiedAlias.Aliases.SingleOrDefault(a => (a.Name == aliasName) && (a.User != null) && (a.User.Id == user.Id)) :
                               null;

                result = (alias != null) ?
                         new ShortUrlResult(CreateShortUrlDTO(alias)) :
                         new ShortUrlResult(new[] { new RuleViolation("alias", TextMessages.SpecifiedAliasIsAlreadyUsedByAnotherUrl) });
            }

            // Alias does not exist
            if ((result.ShortUrl == null) && result.RuleViolations.IsEmpty())
            {
                Alias alias = new Alias { Name = aliasName, IPAddress = ipAddress, CreatedByApi = withApi, ShortUrl = shortUrl, User = user };

                // Associate with url
                shortUrl.Aliases.Add(alias);

                if (user != null)
                {
                    user.Aliases.Add(alias);
                }

                OnCreate(alias);

                result = new ShortUrlResult(CreateShortUrlDTO(alias));
            }

            return result;
        }

        private void OnCreate(Alias alias)
        {
            eventAggregator.GetEvent<ShortUrlCreatedEvent>().Publish(new EventArgs<Alias>(alias));

            ShortUrl shortUrl = alias.ShortUrl;
            SpamStatus status = spamDetectors.Aggregate(shortUrl.SpamStatus, (current, detector) => current | detector.CheckStatus(shortUrl));

            shortUrl.SpamStatus = status;

            if (shortUrl.SpamStatus.IsSpam())
            {
                if (shortUrl.SpamStatus.IsClean())
                {
                    shortUrl.SpamStatus ^= SpamStatus.Clean;
                }

                eventAggregator.GetEvent<PossibleSpamDetectedEvent>().Publish(new EventArgs<Alias>(alias));
            }
        }

        private ShortUrlDTO CreateShortUrlDTO(Alias alias)
        {
            string visitUrl = urlResolver.Absolute(urlResolver.Visit(alias.Name));
            string previewUrl = urlResolver.Absolute(urlResolver.Preview(alias.Name));
            int visits = visitRepository.Count(alias.Name);

            return new ShortUrlDTO(alias, visits, visitUrl, previewUrl);
        }
    }
}