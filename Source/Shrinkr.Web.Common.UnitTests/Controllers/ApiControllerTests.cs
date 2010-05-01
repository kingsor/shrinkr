namespace Shrinkr.Web.UnitTests
{
    using System.Collections.Generic;

    using DataTransferObjects;
    using DomainObjects;
    using Services;

    using Xunit;
    using Moq;
 
    public class ApiControllerTests
    {
        private readonly ApiController controller;
        private readonly Mock<IShortUrlService> shortUrlService;

        public ApiControllerTests()
        {
            shortUrlService = new Mock<IShortUrlService>();
            controller = new ApiController(shortUrlService.Object);
        }

        [Fact]
        public void Create_should_use_short_url_service_to_create_url_by_api_key()
        {
            controller.MockHttpContext("/", "~/Create", "GET");
            shortUrlService.Setup(
                svc =>
                svc.CreateWithApiKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).
                Returns(new ShortUrlResult()).Verifiable();

            controller.Create(new CreateShortUrlApiCommand());

            shortUrlService.VerifyAll();
        }

        [Fact]
        public void Create_should_return_api_result_with_create_url_view_model()
        {
            controller.MockHttpContext("/", "~/Create", "GET");

            var shortUrl = new ShortUrl { Url = "http://shirnkr.com/someurl" };
            var shortUrlDto = new ShortUrlDTO(new Alias { ShortUrl = shortUrl }, 3, "http://shirnkr.com/visit", "http://shirnkr.com/preview");
            var result = new ShortUrlResult(shortUrlDto);

            shortUrlService.Setup(svc => svc.CreateWithApiKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(result);

            Assert.IsType<ApiResult>(controller.Create(new CreateShortUrlApiCommand()));
            Assert.Equal(shortUrl.Url, ((CreateUrlViewModel)controller.ViewData.Model).Url);
        }

        [Fact]
        public void Create_should_add_violation_rules_to_model_errors()
        {
            controller.MockHttpContext("/", "~/Create", "GET");

            var violations = new List<RuleViolation> { new RuleViolation("someParam", "Error Message") };
            var result = new ShortUrlResult(violations);

            shortUrlService.Setup(svc => svc.CreateWithApiKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(result);

            Assert.IsType<ApiResult>(controller.Create(new CreateShortUrlApiCommand()));
            Assert.Equal(1, controller.ViewData.ModelState.Count);
        }
    }
}
