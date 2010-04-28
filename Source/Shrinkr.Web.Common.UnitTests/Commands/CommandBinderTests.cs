namespace Shrinkr.Web.UnitTests
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;
    using Xunit;

    public class CommandBinderTests
    {
        [Fact]
        public void Should_populate_ip_address_url_referrer_and_user_name()
        {
            const string ipAddress = "192.168.0.1";
            const string url = "http://dotnetshoutout.com/";
            const string userName = "joe";

            var httpContext = MvcTestHelper.CreateHttpContext();

            httpContext.SetupGet(c => c.Request.UserHostAddress).Returns(ipAddress);
            httpContext.SetupGet(c => c.Request.Url).Returns(new Uri("http://localhost"));
            httpContext.SetupGet(c => c.Request.UrlReferrer).Returns(new Uri(url));
            httpContext.SetupGet(c => c.Request.ApplicationPath).Returns(string.Empty);
            httpContext.SetupGet(c => c.User.Identity.IsAuthenticated).Returns(true);
            httpContext.SetupGet(c => c.User.Identity.Name).Returns(userName);

            var routeData = new RouteData();
            routeData.Values.Add("controller", "dummy");
            routeData.Values.Add("action", "Process");

            var requestContext = new RequestContext(httpContext.Object, routeData);

            var controller = new DummyController();
            var controllerContext = new ControllerContext(requestContext, controller);
            controller.ControllerContext = controllerContext;

            var bindingContext = new ModelBindingContext
                                     {
                                         ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(DummyUserCommand)),
                                         ValueProvider = new Mock<IValueProvider>().Object,
                                         FallbackToEmptyPrefix = true
                                     };

            var command = (DummyUserCommand) new UserCommandBinder().BindModel(controllerContext, bindingContext);

            Assert.Equal(ipAddress, command.IPAddress);
            Assert.Equal(url, command.Referrer);
            Assert.Equal(userName, command.UserName);
        }

        private class DummyUserCommand : UserCommand
        {
        }

        private class DummyController : Controller
        {
        }
    }
}