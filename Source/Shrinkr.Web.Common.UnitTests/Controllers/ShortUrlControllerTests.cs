namespace Shrinkr.Web.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using MvcExtensions;

    using DataTransferObjects;
    using DomainObjects;
    using Infrastructure;
    using Services;

    using Moq;
    using Xunit;
    using Xunit.Extensions;

    public class ShortUrlControllerTests
    {
        private readonly ShortUrlController controller;
        private readonly Settings settings;
        private readonly Mock<IShortUrlService> shortUrlService;

        public ShortUrlControllerTests()
        {
            new RegisterRoutes().Execute();

            shortUrlService = new Mock<IShortUrlService>();

            settings = new Settings { RedirectPermanently = true, UrlPerPage = 15, BaseType = BaseType.BaseSixtyTwo };

            controller = new ShortUrlController(shortUrlService.Object, settings);
        }

        [Fact]
        public void Create_post_should_use_short_url_service_to_create_url_for_user_name()
        {
            controller.MockHttpContext("/", "~/Create", "POST");
            shortUrlService.Setup(svc => svc.CreateWithUserName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new ShortUrlResult()).Verifiable();

            controller.Create(new CreateShortUrlCommand());

            shortUrlService.VerifyAll();
        }

        [Fact]
        public void Create_post_should_return_adaptive_post_redirect_get_result_with_create_url_view_model()
        {
            controller.MockHttpContext("/", "~/Create", "POST");

            var shortUrl = new ShortUrl {Url = "http://shirnkr.com/someurl"};
            var shortUrlDto = new ShortUrlDTO(new Alias{ ShortUrl = shortUrl}, 3, "http://shirnkr.com/visit", "http://shirnkr.com/preview" );
            var result = new ShortUrlResult(shortUrlDto);

            shortUrlService.Setup(svc => svc.CreateWithUserName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(result);

            Assert.IsType<AdaptivePostRedirectGetResult>(controller.Create(new CreateShortUrlCommand()));
            Assert.Equal(shortUrl.Url, ((CreateUrlViewModel) controller.ViewData.Model).Url);
        }

        [Fact]
        public void Create_post_should_add_violation_rules_to_model_errors()
        {
            controller.MockHttpContext("/", "~/Create", "POST");

            var violations = new List<RuleViolation>{new RuleViolation("someParam","Error Message")};
            var result = new ShortUrlResult(violations);

            shortUrlService.Setup(svc => svc.CreateWithUserName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(result);

            Assert.IsType<AdaptivePostRedirectGetResult>(controller.Create(new CreateShortUrlCommand()));
            Assert.Equal(1, controller.ViewData.ModelState.Count);
        }

        [Fact]
        public void Preview_should_use_short_url_service_to_get_url_result_by_alias()
        {
            controller.MockHttpContext("/", "~/Preview", "GET");

            shortUrlService.Setup(svc => svc.GetByAlias(It.IsAny<string>())).Returns(new ShortUrlResult()).Verifiable();

            controller.Preview(new ShortUrlVisitCommand());

            shortUrlService.VerifyAll();
        }

        [Fact]
        public void Preview_should_return_not_found_result_when_short_url_is_null()
        {
            controller.MockHttpContext("/", "~/Preview", "GET");

            shortUrlService.Setup(svc => svc.GetByAlias(It.IsAny<string>())).Returns(new ShortUrlResult());

            var view = controller.Preview(new ShortUrlVisitCommand());

            Assert.IsType(typeof (NotFoundResult), view);
        }

        [Fact]
        public void Visit_should_use_short_url_service_to_return_visit_result()
        {
            controller.MockHttpContext("/", "~/Visit", "GET");

            shortUrlService.Setup(svc => svc.Visit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new VisitResult()).Verifiable();

            controller.Visit(new ShortUrlVisitCommand { Referrer = "http://shrinkr.com" });

            shortUrlService.VerifyAll();
        }

        [Fact]
        public void Visit_should_return_not_found_result_when_when_visit_is_null()
        {
            controller.MockHttpContext("/", "~/Visit", "GET");

            shortUrlService.Setup(svc => svc.Visit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new VisitResult());

            var result = controller.Visit(new ShortUrlVisitCommand { Referrer = "http://shrinkr.com" });

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Visit_should_add_violation_rules_to_model_errors()
        {
            controller.MockHttpContext("/", "~/Visit", "GET");

            var violations = new List<RuleViolation> { new RuleViolation("someParam", "Error Message") };

            shortUrlService.Setup(svc => svc.Visit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new VisitResult(violations));

            controller.Visit(new ShortUrlVisitCommand { Referrer = "http://shrinkr.com" });

            Assert.Equal(1,controller.ModelState.Count);
        }

        [Fact]
        public void Visit_should_redirect_to_preview_when_url_is_spam()
        {
            controller.MockHttpContext("/", "~/Visit", "GET");

            var shortUrl = new Mock<ShortUrl>();
            shortUrl.SetupGet(s => s.InternalSpamStatus).Returns((int) SpamStatus.Phishing);

            var alias = new Mock<Alias>();
            alias.Setup(a => a.ShortUrl).Returns(shortUrl.Object);

            shortUrlService.Setup(svc => svc.GetByAlias(It.IsAny<string>())).Returns(new ShortUrlResult(new ShortUrlDTO(alias.Object, 1, "http://shrinkr.com/1", "http://shrinkr.com/p/1")));

            var result = (RedirectToRouteResult) controller.Visit(new ShortUrlVisitCommand());

            Assert.Equal("Preview", result.RouteValues.Action());
        }

        [Theory]
        [InlineData(false, typeof(RedirectResult))]
        [InlineData(true, typeof(PermanentRedirectResult))]
        public void Visit_should_redirect_correctly(bool permanent, Type resultType)
        {
            const string url = "http://shrinkr.com/url";

            controller.MockHttpContext("/", "~/Visit", "GET");

            var shortUrl = new Mock<ShortUrl>();
            shortUrl.SetupGet(s => s.InternalSpamStatus).Returns((int)SpamStatus.Clean);
            shortUrl.SetupGet(s => s.Url).Returns(url);

            var alias = new Mock<Alias>();
            alias.Setup(a => a.ShortUrl).Returns(shortUrl.Object);

            shortUrlService.Setup(svc => svc.GetByAlias(It.IsAny<string>())).Returns(new ShortUrlResult(new ShortUrlDTO(alias.Object, 1, "http://shrinkr.com/1", "http://shrinkr.com/p/1")));

            var visit = new Visit { Alias = alias.Object };

            shortUrlService.Setup(svc => svc.Visit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new VisitResult(new VisitDTO(visit)));

            settings.RedirectPermanently = permanent;

            var result = controller.Visit(new ShortUrlVisitCommand());

            Assert.IsType(resultType, result);
            Assert.Equal(url, ((RedirectResult) result).Url);
        }

        [Fact]
        public void List_should_use_short_url_service_to_find_url_list_by_user()
        {
            controller.MockHttpContext("/", "~/List", "GET");

            var alias = new Alias { ShortUrl = new ShortUrl() };
            var pagedResult = new PagedResult<ShortUrlDTO>(new List<ShortUrlDTO> { new ShortUrlDTO(alias, 3, "http://url.com", "http://url.com") }, 10);
            var urlResult = new ShortUrlListResult(pagedResult);

            settings.UrlPerPage = 1;
            shortUrlService.Setup(svc => svc.FindByUser(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(urlResult).Verifiable();

            controller.List(new ShortUrlListCommand());

            shortUrlService.VerifyAll();
        }

        [Fact]
        public void List_should_add_violation_rules_to_model_errors()
        {
            controller.MockHttpContext("/", "~/List", "GET");

            var alias = new Alias {ShortUrl = new ShortUrl()};
            var pagedResult = new PagedResult<ShortUrlDTO>(new List<ShortUrlDTO> { new ShortUrlDTO(alias, 3, "http://url.com", "http://url.com") }, 10);
            var urlResult = new ShortUrlListResult(pagedResult);

            settings.UrlPerPage = 1;
            urlResult.RuleViolations.Add(new RuleViolation("someParam", "Error Message"));

            shortUrlService.Setup(svc => svc.FindByUser(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(urlResult);

            var view = (AdaptiveViewResult) controller.List(new ShortUrlListCommand{Page=1});

            Assert.Equal(1, view.ViewData.ModelState.Count);
        }

        [Fact]
        public void List_should_return_adaptive_view_result()
        {
            controller.MockHttpContext("/", "~/List", "GET");

            var alias = new Alias { ShortUrl = new ShortUrl() };
            var pagedResult = new PagedResult<ShortUrlDTO>(new List<ShortUrlDTO> { new ShortUrlDTO(alias, 3, "http://url.com", "http://url.com") }, 1);
            var urlResult = new ShortUrlListResult(pagedResult);
            settings.UrlPerPage = 1;

            shortUrlService.Setup(svc => svc.FindByUser(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(urlResult);

            var view = (controller.List(new ShortUrlListCommand()));

            Assert.IsType(typeof(AdaptiveViewResult), view);
        }
    }
}