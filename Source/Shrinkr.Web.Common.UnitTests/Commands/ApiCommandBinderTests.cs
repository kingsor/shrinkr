namespace Shrinkr.Web.UnitTests
{
    using System;
    using System.Collections.Specialized;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;
    using Xunit;
    using Xunit.Extensions;

    public class ApiCommandBinderTests
    {
        [Fact]
        public void Should_be_able_to_bind()
        {
            const string IPAddress = "192.168.0.1";
            const string Url = "http://dotnetshoutout.com/";
            const string Alias = "dns";

            var apiKey = Guid.NewGuid().ToString();

            var httpContext = MvcTestHelper.CreateHttpContext();

            httpContext.SetupGet(c => c.Request.UserHostAddress).Returns(IPAddress);
            httpContext.SetupGet(c => c.Request.QueryString).Returns(new NameValueCollection { { "ApiKey", apiKey }, { "Url", Url }, { "Alias", Alias }, { "format", "xml" } });

            var routeData = new RouteData();
            routeData.Values.Add("controller", "dummy");
            routeData.Values.Add("action", "Process");

            var requestContext = new RequestContext(httpContext.Object, routeData);

            var controller = new DummyController();

            var controllerContext = new ControllerContext(requestContext, controller);

            controller.ControllerContext = controllerContext;

            var valueProvider = new QueryStringValueProvider(controllerContext);
            controller.ValueProvider = valueProvider;

            var bindingContext = new ModelBindingContext
                                     {
                                         ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(DummyCommand)),
                                         ValueProvider = valueProvider,
                                         ModelName = "command",
                                         FallbackToEmptyPrefix = true
                                     };

            var command = (DummyCommand)new ApiCommandBinder().BindModel(controllerContext, bindingContext);

            Assert.Equal(apiKey, command.ApiKey);
            Assert.Equal(Url, command.Url);
            Assert.Equal(Alias, command.Alias);
            Assert.Equal(IPAddress, command.IPAddress);
            Assert.Equal(ApiResponseFormat.Xml, command.ResponseFormat);
        }

        [Theory]
        [InlineData("application/json, application/xml, text/plain", ApiResponseFormat.Json)]
        [InlineData("text/json, application/xml, text/plain", ApiResponseFormat.Json)]
        [InlineData("application/xml, text/json, text/plain", ApiResponseFormat.Xml)]
        [InlineData("text/xml, text/json, text/plain", ApiResponseFormat.Xml)]
        [InlineData("text/plain, text/xml, text/json", ApiResponseFormat.Text)]
        [InlineData("", ApiResponseFormat.Text)]
        public void ResultFormat_should_be_detected_from_accept_types_when_no_format_is_specified(string accpetTypes, ApiResponseFormat format)
        {
            var httpContext = MvcTestHelper.CreateHttpContext();

            httpContext.SetupGet(c => c.Request.AcceptTypes).Returns(accpetTypes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            httpContext.SetupGet(c => c.Request.UserHostAddress).Returns("192,168.0.1");

            var routeData = new RouteData();
            routeData.Values.Add("controller", "dummy");
            routeData.Values.Add("action", "Process");

            var requestContext = new RequestContext(httpContext.Object, routeData);

            var valueProvider = new Mock<IValueProvider>();
            var controller = new DummyController { ValueProvider = valueProvider.Object };

            var controllerContext = new ControllerContext(requestContext, controller);

            controller.ControllerContext = controllerContext;

            var bindingContext = new ModelBindingContext
                                     {
                                         ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(DummyCommand)),
                                         ValueProvider = valueProvider.Object,
                                         ModelName = "command",
                                         FallbackToEmptyPrefix = true
                                     };

            var command = (DummyCommand)new ApiCommandBinder().BindModel(controllerContext, bindingContext);

            Assert.Equal(format, command.ResponseFormat);
        }

        private class DummyCommand : CreateShortUrlApiCommand
        {
        }

        private class DummyController : Controller
        {
        }
    }
}