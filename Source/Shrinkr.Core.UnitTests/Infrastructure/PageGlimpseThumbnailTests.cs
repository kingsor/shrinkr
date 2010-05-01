namespace Shrinkr.UnitTests
{
    using System;
    using System.Collections.Generic;

    using Infrastructure;

    using Moq;
    using Xunit;

    public class PageGlimpseThumbnailTests
    {
        private const string ServicePoint = "http://images.pageglimpse.com/v1/thumbnails";
        private const string ApiKey = "myKey";

        private readonly Mock<IHttp> http;
        private readonly PageGlimpseThumbnail thumbnail;

        public PageGlimpseThumbnailTests()
        {
            var settings = new Settings { Thumbnail = new ThumbnailSettings(ApiKey, ServicePoint) };

            http = new Mock<IHttp>();

            thumbnail = new PageGlimpseThumbnail(settings, http.Object);
        }

        [Fact]
        public void UrlFor_should_return_correct_url()
        {
            string absolutePath = thumbnail.UrlFor("http://shrinkr.com", ThumbnailSize.Small);

            Assert.Contains("devkey=myKey", absolutePath);
            Assert.Contains("url=http://shrinkr.com", absolutePath);
            Assert.Contains("size=small", absolutePath);
        }

        [Fact]
        public void Should_be_able_to_capture()
        {
            const string Url = "http://images.pageglimpse.com/v1/thumbnails/request?devkey=myKey&url=http://shrinkr.com";

            http.Setup(h => h.GetAsync(Url, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, string>>(), It.IsAny<IDictionary<string, string>>(), It.IsAny<Action<HttpResponse>>(), It.IsAny<Action<Exception>>())).Verifiable();

            thumbnail.Capture("http://shrinkr.com");

            http.Verify();
        }
    }
}