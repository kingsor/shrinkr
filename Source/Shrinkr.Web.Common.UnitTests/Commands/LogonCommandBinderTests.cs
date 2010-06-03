namespace Shrinkr.Web.UnitTests
{
    using System.Collections.Specialized;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Xunit;
    using Xunit.Extensions;

    public class LogOnCommandBinderTests
    {
        [Theory]
        [InlineData("openid_identifier", "openid_identifier_remember")]
        [InlineData("openid_username", "openid_username_remember")]
        public void Should_be_able_to_bind_for_identifier(string userParameterName, string rememberMeParameterName)
        {
            var form = new NameValueCollection { { userParameterName, "joe" }, { rememberMeParameterName, "true" } };

            var httpContext = MvcTestHelper.CreateHttpContext();

            httpContext.SetupGet(c => c.Request.Form).Returns(form);

            var routeData = new RouteData();
            routeData.Values.Add("controller", "dummy");
            routeData.Values.Add("action", "Process");

            var requestContext = new RequestContext(httpContext.Object, routeData);

            var controller = new DummyController();

            var controllerContext = new ControllerContext(requestContext, controller);

            controller.ControllerContext = controllerContext;

            var valueProvider = new FormValueProvider(controllerContext);
            controller.ValueProvider = valueProvider;

            var bindingContext = new ModelBindingContext
                                     {
                                         ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(DummyCommand)),
                                         ValueProvider = valueProvider,
                                         ModelName = "command",
                                         FallbackToEmptyPrefix = true
                                     };

            var command = (DummyCommand)new LogOnCommandBinder().BindModel(controllerContext, bindingContext);

            Assert.Equal("joe", command.UserName);
            Assert.True(command.RememberMe.HasValue && command.RememberMe.Value);
        }

        private class DummyCommand : LogOnCommand
        {
        }

        private class DummyController : Controller
        {
        }
    }
}