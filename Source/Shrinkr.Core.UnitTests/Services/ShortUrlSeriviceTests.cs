namespace Shrinkr.UnitTests
{
    using System.Collections.Generic;
    using System.Net;

    using DomainObjects;
    using Infrastructure;
    using Repositories;
    using Services;

    using Moq;
    using Xunit;
    using Xunit.Extensions;

    public class ShortUrlSeriviceTests
    {
        private const string Url = "http://dotnetshoutout.com";
        private const string AliasName = "dtntshtt";
        private const string IPAddress = "192.168.0.1";
        private const string UserName = "http://kazimanzurrashid.myopenid.com";
        private const string ApiKey = "B8E5E282-9F06-4272-B7E2-9738C7CFF251";
        private const string Browser = "Internet Explorer 8.0";
        private const string ReferrerUrl = "http://twitter.com/manzurrashid";

        private readonly Mock<IUserRepository> userRepository;
        private readonly Mock<IShortUrlRepository> shortUrlRepository;
        private readonly Mock<IVisitRepository> visitRepository;
        private readonly Mock<IBannedDomainRepository> bannedDomainRepository;
        private readonly Mock<IReservedAliasRepository> reservedAliasRepository;
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly Mock<IExternalContentService> externalContentService;
        private readonly Mock<IThumbnail> thumbnail;
        private readonly Mock<IBaseX> baseX;
        private readonly Mock<IUrlResolver> urlResolver;
        private readonly Mock<ISpamDetector> spamDetector;

        private readonly Mock<ShortUrlCreatedEvent> shortUrlCreatedEvent;
        private readonly Mock<PossibleSpamDetectedEvent> possibleSpamDetectedEvent;
        private readonly Mock<ShortUrlVisitedEvent> shortUrlVisitedEvent;

        private readonly ShortUrlService shortUrlService;

        public ShortUrlSeriviceTests()
        {
            userRepository = new Mock<IUserRepository>();
            shortUrlRepository = new Mock<IShortUrlRepository>();
            visitRepository = new Mock<IVisitRepository>();
            bannedDomainRepository = new Mock<IBannedDomainRepository>();
            reservedAliasRepository = new Mock<IReservedAliasRepository>();
            unitOfWork = new Mock<IUnitOfWork>();
            externalContentService = new Mock<IExternalContentService>();
            thumbnail = new Mock<IThumbnail>();
            baseX = new Mock<IBaseX>();
            urlResolver = new Mock<IUrlResolver>();
            spamDetector = new Mock<ISpamDetector>();

            urlResolver.SetupGet(ur => ur.ApplicationRoot).Returns("http://shrinkr.com");

            shortUrlCreatedEvent = new Mock<ShortUrlCreatedEvent>();
            possibleSpamDetectedEvent = new Mock<PossibleSpamDetectedEvent>();
            shortUrlVisitedEvent = new Mock<ShortUrlVisitedEvent>();

            var eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(ea => ea.GetEvent<ShortUrlCreatedEvent>()).Returns(shortUrlCreatedEvent.Object);
            eventAggregator.Setup(ea => ea.GetEvent<PossibleSpamDetectedEvent>()).Returns(possibleSpamDetectedEvent.Object);
            eventAggregator.Setup(ea => ea.GetEvent<ShortUrlVisitedEvent>()).Returns(shortUrlVisitedEvent.Object);

            shortUrlService = new ShortUrlService(userRepository.Object, shortUrlRepository.Object, visitRepository.Object, bannedDomainRepository.Object, reservedAliasRepository.Object, unitOfWork.Object, externalContentService.Object, thumbnail.Object, baseX.Object, urlResolver.Object, eventAggregator.Object, new[] { spamDetector.Object });
        }

        [Theory]
        [InlineData(UserName, null)]
        [InlineData(null, ApiKey)]
        public void Should_not_be_able_to_create_when_url_is_blank(string userName, string apiKey)
        {
            var result = !string.IsNullOrEmpty(userName) ?
                         shortUrlService.CreateWithUserName(null, AliasName, IPAddress, userName) :
                         shortUrlService.CreateWithApiKey(null, AliasName, IPAddress, apiKey);

            Assert.Equal("url", result.RuleViolations[0].ParameterName);
            Assert.Contains("cannot be blank", result.RuleViolations[0].ErrorMessage);
        }

        [Theory]
        [InlineData(UserName, null)]
        [InlineData(null, ApiKey)]
        public void Should_not_be_able_to_create_when_url_is_not_valid(string userName, string apiKey)
        {
            var result = !string.IsNullOrEmpty(userName) ?
                         shortUrlService.CreateWithUserName("foobar", AliasName, IPAddress, userName) :
                         shortUrlService.CreateWithApiKey("foobar", AliasName, IPAddress, apiKey);

            Assert.Equal("url", result.RuleViolations[0].ParameterName);
            Assert.Contains("valid format", result.RuleViolations[0].ErrorMessage);
        }

        [Theory]
        [InlineData(UserName, null)]
        [InlineData(null, ApiKey)]
        public void Should_not_be_able_to_create_for_own_domain(string userName, string apiKey)
        {
            urlResolver.Setup(ur => ur.ApplicationRoot).Returns("http://shrinkr.com");

            var result = !string.IsNullOrEmpty(userName) ?
                         shortUrlService.CreateWithUserName("http://shrinkr.com/1", AliasName, IPAddress, userName) :
                         shortUrlService.CreateWithApiKey("http://shrinkr.com/1", AliasName, IPAddress, apiKey);

            Assert.Equal("url", result.RuleViolations[0].ParameterName);
            Assert.Contains("its own domain", result.RuleViolations[0].ErrorMessage);
        }

        [Theory]
        [InlineData(UserName, null)]
        [InlineData(null, ApiKey)]
        public void Should_not_be_able_to_create_when_url_matches_exists_in_banned_domains(string userName, string apiKey)
        {
            bannedDomainRepository.Setup(r => r.IsMatching(It.IsAny<string>())).Returns(true);

            var result = !string.IsNullOrEmpty(userName) ?
                         shortUrlService.CreateWithUserName(Url, AliasName, IPAddress, userName) :
                         shortUrlService.CreateWithApiKey(Url, AliasName, IPAddress, apiKey);

            Assert.Equal("url", result.RuleViolations[0].ParameterName);
            Assert.Contains("banned domains", result.RuleViolations[0].ErrorMessage);
        }

        [Theory]
        [InlineData(UserName, null)]
        [InlineData(null, ApiKey)]
        public void Should_not_be_able_to_create_when_alias_does_not_matches_with_allowed_characters(string userName, string apiKey)
        {
            var result = !string.IsNullOrEmpty(userName) ?
                         shortUrlService.CreateWithUserName(Url, AliasName, IPAddress, userName) :
                         shortUrlService.CreateWithApiKey(Url, AliasName, IPAddress, apiKey);

            Assert.Equal("alias", result.RuleViolations[0].ParameterName);
            Assert.Contains("alphanumeric characters", result.RuleViolations[0].ErrorMessage);
        }

        [Theory]
        [InlineData(UserName, null)]
        [InlineData(null, ApiKey)]
        public void Should_not_be_able_to_create_when_alias_matches_with_reserved_alias(string userName, string apiKey)
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);
            reservedAliasRepository.Setup(r => r.IsMatching(It.IsAny<string>())).Returns(true);

            var result = !string.IsNullOrEmpty(userName) ?
                         shortUrlService.CreateWithUserName(Url, AliasName, IPAddress, userName) :
                         shortUrlService.CreateWithApiKey(Url, AliasName, IPAddress, apiKey);

            Assert.Equal("alias", result.RuleViolations[0].ParameterName);
            Assert.Contains("reserved alias", result.RuleViolations[0].ErrorMessage);
        }

        [Theory]
        [InlineData(UserName, null)]
        [InlineData(null, ApiKey)]
        public void Should_not_be_able_to_create_when_IPAddress_is_blank(string userName, string apiKey)
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var result = !string.IsNullOrEmpty(userName) ?
                         shortUrlService.CreateWithUserName(Url, AliasName, null, userName) :
                         shortUrlService.CreateWithApiKey(Url, AliasName, null, apiKey);

            Assert.Equal("ipAddress", result.RuleViolations[0].ParameterName);
            Assert.Contains("cannot be blank", result.RuleViolations[0].ErrorMessage);
        }

        [Theory]
        [InlineData(UserName, null)]
        [InlineData(null, ApiKey)]
        public void Should_not_be_able_to_create_when_IPAddress_is_not_valid(string userName, string apiKey)
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var result = !string.IsNullOrEmpty(userName) ?
                         shortUrlService.CreateWithUserName(Url, AliasName, "256.0.256.0", userName) :
                         shortUrlService.CreateWithApiKey(Url, AliasName, "256.0.256.0", apiKey);

            Assert.Equal("ipAddress", result.RuleViolations[0].ParameterName);
            Assert.Contains("not in valid format", result.RuleViolations[0].ErrorMessage);
        }

        [Fact]
        public void Should_not_be_able_to_create_with_user_name_when_user_does_not_exist()
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var result = shortUrlService.CreateWithUserName(Url, AliasName, IPAddress, UserName);

            Assert.Equal("userName", result.RuleViolations[0].ParameterName);
            Assert.Contains("user does not exist", result.RuleViolations[0].ErrorMessage);
        }

        [Fact]
        public void Should_not_be_able_to_create_with_user_name_when_user_is_locked()
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var user = new Mock<User>();
            user.SetupGet(u => u.IsLockedOut).Returns(true);

            userRepository.Setup(r => r.GetByName(It.IsAny<string>())).Returns(user.Object);

            var result = shortUrlService.CreateWithUserName(Url, AliasName, IPAddress, UserName);

            Assert.Equal("userName", result.RuleViolations[0].ParameterName);
            Assert.Contains("currently locked out", result.RuleViolations[0].ErrorMessage);
        }

        [Fact]
        public void Should_not_be_able_to_create_with_api_key_when_user_does_not_exist()
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var result = shortUrlService.CreateWithApiKey(Url, AliasName, IPAddress, ApiKey);

            Assert.Equal("apiKey", result.RuleViolations[0].ParameterName);
            Assert.Contains("Invalid api key ", result.RuleViolations[0].ErrorMessage);
        }

        [Fact]
        public void Should_not_be_able_to_create_with_api_key_when_user_is_locked()
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var user = new Mock<User>();
            user.SetupGet(u => u.IsLockedOut).Returns(true);

            userRepository.Setup(r => r.GetByApiKey(It.IsAny<string>())).Returns(user.Object);

            var result = shortUrlService.CreateWithApiKey(Url, AliasName, IPAddress, ApiKey);

            Assert.Equal("apiKey", result.RuleViolations[0].ParameterName);
            Assert.Contains("currently locked out", result.RuleViolations[0].ErrorMessage);
        }

        [Fact]
        public void Should_not_be_able_to_create_with_api_key_when_api_access_is_not_allowed()
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var user = new Mock<User>();
            user.SetupGet(u => u.CanAccessApi).Returns(false);

            userRepository.Setup(r => r.GetByApiKey(It.IsAny<string>())).Returns(user.Object);

            var result = shortUrlService.CreateWithApiKey(Url, AliasName, IPAddress, UserName);

            Assert.Equal("apiKey", result.RuleViolations[0].ParameterName);
            Assert.Contains("not allowed to access", result.RuleViolations[0].ErrorMessage);
        }

        [Fact]
        public void Should_not_be_able_to_create_with_api_key_when_daily_limit_has_exceeded()
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var user = new Mock<User>();
            user.SetupGet(u => u.CanAccessApi).Returns(true);
            user.Setup(u => u.HasExceededDailyLimit()).Returns(true);

            userRepository.Setup(r => r.GetByApiKey(It.IsAny<string>())).Returns(user.Object);

            var result = shortUrlService.CreateWithApiKey(Url, AliasName, IPAddress, UserName);

            Assert.Equal("apiKey", result.RuleViolations[0].ParameterName);
            Assert.Contains("already reached daily limit", result.RuleViolations[0].ErrorMessage);
        }

        [Theory]
        [InlineData(UserName, null)]
        [InlineData(null, ApiKey)]
        public void Should_not_be_able_to_create_when_url_throws_web_exception(string userName, string apiKey)
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var user = new Mock<User>();
            user.SetupGet(u => u.CanAccessApi).Returns(true);

            userRepository.Setup(r => r.GetByName(It.IsAny<string>())).Returns(user.Object);
            userRepository.Setup(r => r.GetByApiKey(It.IsAny<string>())).Returns(user.Object);

            externalContentService.Setup(ecs => ecs.Retrieve(It.IsAny<string>())).Throws(new WebException("Url does not exits."));

            var result = !string.IsNullOrEmpty(userName) ?
                         shortUrlService.CreateWithUserName(Url, AliasName, IPAddress, userName) :
                         shortUrlService.CreateWithApiKey(Url, AliasName, IPAddress, apiKey);

            Assert.Equal("url", result.RuleViolations[0].ParameterName);
            Assert.Contains("Url does not exits", result.RuleViolations[0].ErrorMessage);
        }

        [Theory]
        [InlineData(UserName, null)]
        [InlineData(null, ApiKey)]
        public void Should_be_able_to_create_for_new_url_when_alias_is_not_specified(string userName, string apiKey)
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var user = new Mock<User>();
            user.SetupGet(u => u.CanAccessApi).Returns(true);
            user.Setup(u => u.Aliases).Returns(new List<Alias>());

            userRepository.Setup(r => r.GetByName(It.IsAny<string>())).Returns(user.Object);
            userRepository.Setup(r => r.GetByApiKey(It.IsAny<string>())).Returns(user.Object);

            externalContentService.Setup(ecs => ecs.Retrieve(It.IsAny<string>())).Returns(new ExternalContent("dummy title", "dummy context"));

            urlResolver.Setup(ur => ur.Absolute(It.IsAny<string>())).Returns((string url) => url);
            urlResolver.Setup(ur => ur.Visit(It.IsAny<string>())).Returns("http://shrinkr.com/1");
            urlResolver.Setup(ur => ur.Preview(It.IsAny<string>())).Returns("http://shrinkr.com/p/1");

            var result = !string.IsNullOrEmpty(userName) ?
                         shortUrlService.CreateWithUserName(Url, null, IPAddress, userName) :
                         shortUrlService.CreateWithApiKey(Url, null, IPAddress, apiKey);

            Assert.Empty(result.RuleViolations);
            Assert.NotNull(result.ShortUrl);
        }

        [Theory]
        [InlineData(UserName, null)]
        [InlineData(null, ApiKey)]
        public void Should_be_able_to_create_for_existing_url_when_alias_is_not_specified(string userName, string apiKey)
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var user = new Mock<User>();
            user.SetupGet(u => u.CanAccessApi).Returns(true);
            user.Setup(u => u.Aliases).Returns(new List<Alias>());

            userRepository.Setup(r => r.GetByName(It.IsAny<string>())).Returns(user.Object);
            userRepository.Setup(r => r.GetByApiKey(It.IsAny<string>())).Returns(user.Object);

            var shortUrl = new Mock<ShortUrl>();
            shortUrl.SetupGet(su => su.Id).Returns(1);
            shortUrl.SetupGet(su => su.Aliases).Returns(new List<Alias>());

            shortUrlRepository.Setup(r => r.GetByHash(It.IsAny<string>())).Returns(shortUrl.Object);

            urlResolver.Setup(ur => ur.Absolute(It.IsAny<string>())).Returns((string url) => url);
            urlResolver.Setup(ur => ur.Visit(It.IsAny<string>())).Returns("http://shrinkr.com/1");
            urlResolver.Setup(ur => ur.Preview(It.IsAny<string>())).Returns("http://shrinkr.com/p/1");

            var result = !string.IsNullOrEmpty(userName) ?
                         shortUrlService.CreateWithUserName(Url, null, IPAddress, userName) :
                         shortUrlService.CreateWithApiKey(Url, null, IPAddress, apiKey);

            Assert.Empty(result.RuleViolations);
            Assert.NotNull(result.ShortUrl);
        }

        [Fact]
        public void Should_be_able_to_create_for_existing_url_when_user_and_alias_is_not_specified()
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var shortUrl = new Mock<ShortUrl>();
            shortUrl.SetupGet(su => su.Id).Returns(1);
            shortUrl.SetupGet(su => su.Aliases).Returns(new List<Alias>());

            shortUrlRepository.Setup(r => r.GetByHash(It.IsAny<string>())).Returns(shortUrl.Object);

            urlResolver.Setup(ur => ur.Absolute(It.IsAny<string>())).Returns((string url) => url);
            urlResolver.Setup(ur => ur.Visit(It.IsAny<string>())).Returns("http://shrinkr.com/1");
            urlResolver.Setup(ur => ur.Preview(It.IsAny<string>())).Returns("http://shrinkr.com/p/1");

            var result = shortUrlService.CreateWithUserName(Url, null, IPAddress, null);

            Assert.Empty(result.RuleViolations);
            Assert.NotNull(result.ShortUrl);
        }

        [Theory]
        [InlineData(UserName, null)]
        [InlineData(null, ApiKey)]
        public void Should_be_able_to_create_for_existing_url_when_user_is_specified(string userName, string apiKey)
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var user = new Mock<User>();
            user.SetupGet(u => u.Id).Returns(1);
            user.SetupGet(u => u.CanAccessApi).Returns(true);
            user.Setup(u => u.Aliases).Returns(new List<Alias>());

            userRepository.Setup(r => r.GetByName(It.IsAny<string>())).Returns(user.Object);
            userRepository.Setup(r => r.GetByApiKey(It.IsAny<string>())).Returns(user.Object);

            var shortUrl = new Mock<ShortUrl>();
            shortUrl.SetupGet(su => su.Id).Returns(1);
            shortUrl.SetupGet(su => su.Aliases).Returns(new List<Alias> { new Alias { Name = "foo", User = user.Object, ShortUrl = shortUrl.Object } });

            shortUrlRepository.Setup(r => r.GetByHash(It.IsAny<string>())).Returns(shortUrl.Object);

            urlResolver.Setup(ur => ur.Absolute(It.IsAny<string>())).Returns((string url) => url);
            urlResolver.Setup(ur => ur.Visit(It.IsAny<string>())).Returns("http://shrinkr.com/1");
            urlResolver.Setup(ur => ur.Preview(It.IsAny<string>())).Returns("http://shrinkr.com/p/1");

            var result = !string.IsNullOrEmpty(userName) ?
                         shortUrlService.CreateWithUserName(Url, null, IPAddress, userName) :
                         shortUrlService.CreateWithApiKey(Url, null, IPAddress, apiKey);

            Assert.Empty(result.RuleViolations);
            Assert.NotNull(result.ShortUrl);
        }

        [Theory]
        [InlineData(UserName, null)]
        [InlineData(null, ApiKey)]
        public void Should_be_able_to_create_for_existing_url_when_user_and_alias_is_specified(string userName, string apiKey)
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var user = new Mock<User>();
            user.SetupGet(u => u.Id).Returns(1);
            user.SetupGet(u => u.CanAccessApi).Returns(true);
            user.Setup(u => u.Aliases).Returns(new List<Alias>());

            userRepository.Setup(r => r.GetByName(It.IsAny<string>())).Returns(user.Object);
            userRepository.Setup(r => r.GetByApiKey(It.IsAny<string>())).Returns(user.Object);

            var shortUrl = new Mock<ShortUrl>();
            shortUrl.SetupGet(su => su.Id).Returns(1);
            shortUrl.SetupGet(su => su.Aliases).Returns(new List<Alias> { new Alias { Name = AliasName, User = user.Object, ShortUrl = shortUrl.Object } });

            shortUrlRepository.Setup(r => r.GetByHash(It.IsAny<string>())).Returns(shortUrl.Object);
            shortUrlRepository.Setup(r => r.GetByAliasName(It.IsAny<string>())).Returns(shortUrl.Object);

            urlResolver.Setup(ur => ur.Absolute(It.IsAny<string>())).Returns((string url) => url);
            urlResolver.Setup(ur => ur.Visit(It.IsAny<string>())).Returns("http://shrinkr.com/1");
            urlResolver.Setup(ur => ur.Preview(It.IsAny<string>())).Returns("http://shrinkr.com/p/1");

            var result = !string.IsNullOrEmpty(userName) ?
                         shortUrlService.CreateWithUserName(Url, AliasName, IPAddress, userName) :
                         shortUrlService.CreateWithApiKey(Url, AliasName, IPAddress, apiKey);

            Assert.Empty(result.RuleViolations);
            Assert.NotNull(result.ShortUrl);
        }

        [Theory]
        [InlineData(UserName, null)]
        [InlineData(null, ApiKey)]
        public void Create_should_publish_url_created_event(string userName, string apiKey)
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var user = new Mock<User>();
            user.SetupGet(u => u.CanAccessApi).Returns(true);
            user.Setup(u => u.Aliases).Returns(new List<Alias>());

            userRepository.Setup(r => r.GetByName(It.IsAny<string>())).Returns(user.Object);
            userRepository.Setup(r => r.GetByApiKey(It.IsAny<string>())).Returns(user.Object);

            externalContentService.Setup(ecs => ecs.Retrieve(It.IsAny<string>())).Returns(new ExternalContent("dummy title", "dummy context"));

            urlResolver.Setup(ur => ur.Absolute(It.IsAny<string>())).Returns((string url) => url);
            urlResolver.Setup(ur => ur.Visit(It.IsAny<string>())).Returns("http://shrinkr.com/1");
            urlResolver.Setup(ur => ur.Preview(It.IsAny<string>())).Returns("http://shrinkr.com/p/1");

            shortUrlCreatedEvent.Setup(evt => evt.Publish(It.IsAny<EventArgs<Alias>>())).Verifiable();

            if (!string.IsNullOrEmpty(userName))
            {
                shortUrlService.CreateWithUserName(Url, null, IPAddress, userName);
            }
            else
            {
                shortUrlService.CreateWithApiKey(Url, null, IPAddress, apiKey);
            }

            shortUrlCreatedEvent.Verify();
        }

        [Theory]
        [InlineData(UserName, null)]
        [InlineData(null, ApiKey)]
        public void Create_should_change_spam_status_when_spam_detectors_detects_as_spam(string userName, string apiKey)
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var user = new Mock<User>();
            user.SetupGet(u => u.CanAccessApi).Returns(true);
            user.Setup(u => u.Aliases).Returns(new List<Alias>());

            userRepository.Setup(r => r.GetByName(It.IsAny<string>())).Returns(user.Object);
            userRepository.Setup(r => r.GetByApiKey(It.IsAny<string>())).Returns(user.Object);

            externalContentService.Setup(ecs => ecs.Retrieve(It.IsAny<string>())).Returns(new ExternalContent("dummy title", "dummy context"));

            urlResolver.Setup(ur => ur.Absolute(It.IsAny<string>())).Returns((string url) => url);
            urlResolver.Setup(ur => ur.Visit(It.IsAny<string>())).Returns("http://shrinkr.com/1");
            urlResolver.Setup(ur => ur.Preview(It.IsAny<string>())).Returns("http://shrinkr.com/p/1");

            spamDetector.Setup(sd => sd.CheckStatus(It.IsAny<ShortUrl>())).Returns(SpamStatus.BadWord);

            var result = !string.IsNullOrEmpty(userName) ?
                         shortUrlService.CreateWithUserName(Url, null, IPAddress, userName) :
                         shortUrlService.CreateWithApiKey(Url, null, IPAddress, apiKey);

            Assert.Equal(SpamStatus.BadWord, result.ShortUrl.SpamStatus);
        }

        [Theory]
        [InlineData(UserName, null)]
        [InlineData(null, ApiKey)]
        public void Create_should_publish_possible_spam_dectecd_event_when_spam_detectors_detects_as_spam(string userName, string apiKey)
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var user = new Mock<User>();
            user.SetupGet(u => u.CanAccessApi).Returns(true);
            user.Setup(u => u.Aliases).Returns(new List<Alias>());

            userRepository.Setup(r => r.GetByName(It.IsAny<string>())).Returns(user.Object);
            userRepository.Setup(r => r.GetByApiKey(It.IsAny<string>())).Returns(user.Object);

            externalContentService.Setup(ecs => ecs.Retrieve(It.IsAny<string>())).Returns(new ExternalContent("dummy title", "dummy context"));

            urlResolver.Setup(ur => ur.Absolute(It.IsAny<string>())).Returns((string url) => url);
            urlResolver.Setup(ur => ur.Visit(It.IsAny<string>())).Returns("http://shrinkr.com/1");
            urlResolver.Setup(ur => ur.Preview(It.IsAny<string>())).Returns("http://shrinkr.com/p/1");

            spamDetector.Setup(sd => sd.CheckStatus(It.IsAny<ShortUrl>())).Returns(SpamStatus.BadWord);

            possibleSpamDetectedEvent.Setup(evt => evt.Publish(It.IsAny<EventArgs<Alias>>())).Verifiable();

            if (!string.IsNullOrEmpty(userName))
            {
                shortUrlService.CreateWithUserName(Url, null, IPAddress, userName);
            }
            else
            {
                shortUrlService.CreateWithApiKey(Url, null, IPAddress, apiKey);
            }

            possibleSpamDetectedEvent.Verify();
        }

        [Fact]
        public void Should_not_be_able_to_get_by_alias_when_blank_alias_is_specified()
        {
            var result = shortUrlService.GetByAlias(null);

            Assert.Equal("alias", result.RuleViolations[0].ParameterName);
        }

        [Fact]
        public void Should_not_be_able_to_get_by_alias_when_alias_does_not_matches_with_allowed_characters()
        {
            var result = shortUrlService.GetByAlias("xyz");

            Assert.Equal("alias", result.RuleViolations[0].ParameterName);
        }

        [Fact]
        public void Should_be_able_to_get_by_alias()
        {
            baseX.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            var shortUrl = new Mock<ShortUrl>();
            shortUrl.SetupGet(su => su.Aliases).Returns(new List<Alias> { new Alias { Name = AliasName, ShortUrl = shortUrl.Object } });

            shortUrlRepository.Setup(repository => repository.GetByAliasName(It.IsAny<string>())).Returns(shortUrl.Object);

            urlResolver.Setup(ur => ur.Absolute(It.IsAny<string>())).Returns((string url) => url);
            urlResolver.Setup(ur => ur.Visit(It.IsAny<string>())).Returns("http://shrinkr.com/1");
            urlResolver.Setup(ur => ur.Preview(It.IsAny<string>())).Returns("http://shrinkr.com/p/1");

            var urlResult = shortUrlService.GetByAlias(AliasName);

            Assert.Empty(urlResult.RuleViolations);
            Assert.Equal(AliasName, urlResult.ShortUrl.Alias);
        }

        [Fact]
        public void Should_not_be_able_to_visit_when_alias_is_blank()
        {
            var result = shortUrlService.Visit(null, IPAddress, Browser, ReferrerUrl);

            Assert.Equal("alias", result.RuleViolations[0].ParameterName);
        }

        [Fact]
        public void Should_not_be_able_to_visit_when_alias_does_not_matches_with_allowed_characters()
        {
            var result = shortUrlService.Visit(AliasName, IPAddress, Browser, ReferrerUrl);

            Assert.Equal("alias", result.RuleViolations[0].ParameterName);
        }

        [Fact]
        public void Should_not_be_able_to_visit_when_ip_address_is_blank()
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var result = shortUrlService.Visit(AliasName, null, Browser, ReferrerUrl);

            Assert.Equal("ipAddress", result.RuleViolations[0].ParameterName);
        }

        [Fact]
        public void Should_not_be_able_to_visit_when_ip_address_is_not_valid()
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var result = shortUrlService.Visit(AliasName, "256.0.0.0", Browser, ReferrerUrl);

            Assert.Equal("ipAddress", result.RuleViolations[0].ParameterName);
        }

        [Fact]
        public void Should_not_be_able_to_visit_when_alias_does_not_exist()
        {
            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var result = shortUrlService.Visit(AliasName, IPAddress, Browser, ReferrerUrl);

            Assert.Equal("alias", result.RuleViolations[0].ParameterName);
        }

        [Fact]
        public void Should_be_able_to_visit()
        {
            var alias = new Mock<Alias>();
            alias.SetupGet(a => a.Name).Returns(AliasName);

            var shortUrl = new Mock<ShortUrl>();
            shortUrl.SetupGet(su => su.Aliases).Returns(new List<Alias> { alias.Object });

            alias.SetupGet(a => a.ShortUrl).Returns(shortUrl.Object);
            shortUrlRepository.Setup(r => r.GetByAliasName(It.IsAny<string>())).Returns(shortUrl.Object);

            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            var result = shortUrlService.Visit(AliasName, IPAddress, Browser, ReferrerUrl);

            Assert.Empty(result.RuleViolations);
            Assert.NotNull(result.Visit);
        }

        [Fact]
        public void Visit_should_publish_short_url_visited_event()
        {
            var alias = new Mock<Alias>();
            alias.SetupGet(a => a.Name).Returns(AliasName);

            var shortUrl = new Mock<ShortUrl>();
            shortUrl.SetupGet(su => su.Aliases).Returns(new List<Alias> { alias.Object });

            alias.SetupGet(a => a.ShortUrl).Returns(shortUrl.Object);
            shortUrlRepository.Setup(r => r.GetByAliasName(It.IsAny<string>())).Returns(shortUrl.Object);

            baseX.Setup(b => b.IsValid(It.IsAny<string>())).Returns(true);

            shortUrlVisitedEvent.Setup(evt => evt.Publish(It.IsAny<EventArgs<Visit>>())).Verifiable();

            shortUrlService.Visit(AliasName, IPAddress, Browser, ReferrerUrl);

            shortUrlVisitedEvent.Verify();
        }

        [Fact]
        public void Should_not_be_able_to_find_by_user_when_user_name_is_blank()
        {
            var urlListResult = shortUrlService.FindByUser(null, 0, 10);

            Assert.Equal("userName", urlListResult.RuleViolations[0].ParameterName);
        }

        [Fact]
        public void Should_not_be_able_to_find_by_user_when_start_index_is_negative()
        {
            var urlListResult = shortUrlService.FindByUser(UserName, -1, 10);

            Assert.Equal("start", urlListResult.RuleViolations[0].ParameterName);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Should_not_be_able_to_find_by_user_when_max_is_negative_or_zero(int max)
        {
            var urlListResult = shortUrlService.FindByUser(UserName, 0, max);

            Assert.Equal("max", urlListResult.RuleViolations[0].ParameterName);
        }

        [Fact]
        public void Should_not_be_able_to_find_by_user_when_user_does_not_exist()
        {
            var urlListResult = shortUrlService.FindByUser(UserName, 0, 10);

            Assert.Equal("userName", urlListResult.RuleViolations[0].ParameterName);
        }

        [Fact]
        public void Should_not_be_able_to_find_by_user_when_user_is_locked_out()
        {
            var user = new Mock<User>();
            user.SetupGet(u => u.IsLockedOut).Returns(true);

            userRepository.Setup(r => r.GetByName(It.IsAny<string>())).Returns(user.Object);

            var urlListResult = shortUrlService.FindByUser(UserName, 0, 10);

            Assert.Equal("userName", urlListResult.RuleViolations[0].ParameterName);
        }

        [Fact]
        public void Should_be_able_to_find_by_user()
        {
            const int ShortUrlCount = 5;
            const long Id = long.MaxValue;

            var user = new Mock<User>();
            user.SetupGet(u => u.Id).Returns(Id);

            userRepository.Setup(repository => repository.GetByName(It.IsAny<string>())).Returns(user.Object);

            var shortUrls = new List<ShortUrl>();

            for (var i = 1; i <= ShortUrlCount; i++)
            {
                var shortUrl = new Mock<ShortUrl>();
                var aliases = new List<Alias> { new Alias { Name = AliasName, User = user.Object, ShortUrl = shortUrl.Object } };

                shortUrl.SetupGet(su => su.Aliases).Returns(aliases);

                shortUrls.Add(shortUrl.Object);
            }

            var pagedResult = new PagedResult<ShortUrl>(shortUrls, 10);

            shortUrlRepository.Setup(repository => repository.FindByUserId(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).Returns(pagedResult);

            urlResolver.Setup(ur => ur.Absolute(It.IsAny<string>())).Returns((string url) => url);
            urlResolver.Setup(ur => ur.Visit(It.IsAny<string>())).Returns("http://shrinkr.com/1");
            urlResolver.Setup(ur => ur.Preview(It.IsAny<string>())).Returns("http://shrinkr.com/p/1");

            var urlListResult = shortUrlService.FindByUser(UserName, 0, 10);

            Assert.Empty(urlListResult.RuleViolations);
            Assert.Equal(ShortUrlCount, urlListResult.ShortUrls.Count);
            Assert.Equal(10, urlListResult.Total);
        }
    }
}