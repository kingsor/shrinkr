namespace Shrinkr.Web.UnitTests
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using DataTransferObjects;
    using DomainObjects;

    using Moq;
    using Xunit;
    using Xunit.Extensions;

    public class ApiResultTests
    {
        private readonly Mock<HttpContextBase> httpContext;
        private readonly ControllerContext controllerContext;
        private readonly ApiResult apiResult;

        public ApiResultTests()
        {
            apiResult = new ApiResult(ApiResponseFormat.Text);

            httpContext = MvcTestHelper.CreateHttpContext();

            controllerContext = new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
        }

        [Theory]
        [InlineData(ApiResponseFormat.Json, "application/json")]
        [InlineData(ApiResponseFormat.Xml, "application/xml")]
        [InlineData(ApiResponseFormat.Text, "text/plain")]
        public void ExecuteResult_should_set_response_content_type_to_correct_response_format_type(ApiResponseFormat responseFormat, string expectedContentType)
        {
            var result = new ApiResult(responseFormat);

            result.ExecuteResult(controllerContext);

            httpContext.VerifySet(c => c.Response.ContentType = expectedContentType);
        }

        [Fact]
        public void ExecuteResult_should_clear_response()
        {
            apiResult.ExecuteResult(controllerContext);

            httpContext.Verify(c => c.Response.Clear());
        }

        [Theory]
        [InlineData(ApiResponseFormat.Json, "{\"shortUrl\":\"http:\\/\\/shrinkr.com\\/msdn\",\"previewUrl\":\"http:\\/\\/shrinkr.com\\/Preview\\/msdn\",\"alias\":\"MSDN\",\"longUrl\":\"http:\\/\\/msdn.microsoft.com\\/\",\"title\":\"MSDN\"}")]
        [InlineData(ApiResponseFormat.Xml, "<create xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><shortUrl>http://shrinkr.com/msdn</shortUrl><previewUrl>http://shrinkr.com/Preview/msdn</previewUrl><alias>MSDN</alias><longUrl>http://msdn.microsoft.com/</longUrl><title>MSDN</title></create>")]
        [InlineData(ApiResponseFormat.Text, "http://shrinkr.com/msdn")]
        public void ExecuteResult_should_write_correct_content_to_response(ApiResponseFormat responseFormat, string content)
        {
            var alias = new Alias { Name = "MSDN", ShortUrl = new ShortUrl { Title = "MSDN", Url = "http://msdn.microsoft.com/" } };
            var shortUrlDto = new ShortUrlDTO(alias, 3, "http://shrinkr.com/msdn", "http://shrinkr.com/Preview/msdn");
            var viewModel = new CreateUrlViewModel(shortUrlDto);

            controllerContext.Controller.ViewData = new ViewDataDictionary(viewModel);

            var result = new ApiResult(responseFormat);

            result.ExecuteResult(controllerContext);

            httpContext.Verify(c => c.Response.Write(content));
        }
    }
}