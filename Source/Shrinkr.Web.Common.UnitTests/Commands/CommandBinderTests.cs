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
            const string IPAddress = "192.168.0.1";
            const string Url = "http://dotnetshoutout.com/";
            const string UserName = "joe";

            var httpContext = MvcTestHelper.CreateHttpContext();

            httpContext.SetupGet(c => c.Request.UserHostAddress).Returns(IPAddress);
            httpContext.SetupGet(c => c.Request.Url).Returns(new Uri("http://localhost"));
            httpContext.SetupGet(c => c.Request.UrlReferrer).Returns(new Uri(Url));
            httpContext.SetupGet(c => c.Request.ApplicationPath).Returns(string.Empty);
            httpContext.SetupGet(c => c.User.Identity.IsAuthenticated).Returns(true);
            httpContext.SetupGet(c => c.User.Identity.Name).Returns(UserName);

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

            Assert.Equal(IPAddress, command.IPAddress);
            Assert.Equal(Url, command.Referrer);
            Assert.Equal(UserName, command.UserName);
        }

        private class DummyUserCommand : UserCommand
        {
        }

        private class DummyController : Controller
        {
        }
    }
}