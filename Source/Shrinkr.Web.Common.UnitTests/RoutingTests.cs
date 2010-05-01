namespace Shrinkr.Web.UnitTests
{
    using System.Web.Routing;

    using Microsoft.Practices.ServiceLocation;

    using Moq;
    using Xunit;

    public class RoutingTests
    {
        private readonly RouteCollection routes;

        public RoutingTests()
        {
            routes = new RouteCollection();

            var serviceLocator = new Mock<IServiceLocator>();

            serviceLocator.Setup(sl => sl.GetInstance<RouteCollection>()).Returns(routes);

            new ConfigureRoutes().Execute(serviceLocator.Object);
        }

        [Fact]
        public void Should_route_existing_file()
        {
            Assert.True(routes.RouteExistingFiles);
        }

        [Fact]
        public void Axd_files_should_not_be_routed()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("~/asset.axd");
            var routeData = routes.GetRouteData(httpContext.Object);

            Assert.Null(routeData.Controller());
            Assert.Null(routeData.Action());
        }

        [Fact]
        public void Content_folder_should_not_be_routed()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("~/Content/mainbg.jpg");
            var routeData = routes.GetRouteData(httpContext.Object);

            Assert.Null(routeData.Controller());
            Assert.Null(routeData.Action());
        }

        [Fact]
        public void Scripts_folder_should_not_be_routed()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("~/Scripts/script.js");
            var routeData = routes.GetRouteData(httpContext.Object);

            Assert.Null(routeData.Controller());
            Assert.Null(routeData.Action());
        }

        [Fact]
        public void FavIcon_should_not_be_routed()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("~/favicon.ico");
            var routeData = routes.GetRouteData(httpContext.Object);

            Assert.Null(routeData.Controller());
            Assert.Null(routeData.Action());
        }

        [Fact]
        public void Home_should_be_routed_to_shorturl_create()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("~/");
            var routeData = routes.GetRouteData(httpContext.Object);

            Assert.Equal("ShortUrl", routeData.Controller());
            Assert.Equal("Create", routeData.Action());
        }

        [Fact]
        public void Xrds_should_be_routed_to_authentication_xrds()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("~/Xrds");
            var routeData = routes.GetRouteData(httpContext.Object);

            Assert.Equal("Authentication", routeData.Controller());
            Assert.Equal("Xrds", routeData.Action());
        }

        [Fact]
        public void LogOn_should_be_routed_to_authentication_logon()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("~/LogOn");
            var routeData = routes.GetRouteData(httpContext.Object);

            Assert.Equal("Authentication", routeData.Controller());
            Assert.Equal("LogOn", routeData.Action());
        }

        [Fact]
        public void LogOff_should_be_routed_to_authentication_logoff()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("~/LogOff");
            var routeData = routes.GetRouteData(httpContext.Object);

            Assert.Equal("Authentication", routeData.Controller());
            Assert.Equal("LogOff", routeData.Action());
        }

        [Fact]
        public void MyProfile_should_be_routed_to_user_profile()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("~/MyProfile");
            var routeData = routes.GetRouteData(httpContext.Object);

            Assert.Equal("User", routeData.Controller());
            Assert.Equal("Profile", routeData.Action());
        }

        [Fact]
        public void GenerateKey_should_be_routed_to_user_generatekey()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("~/GenerateKey");
            var routeData = routes.GetRouteData(httpContext.Object);

            Assert.Equal("User", routeData.Controller());
            Assert.Equal("GenerateKey", routeData.Action());
        }

        [Fact]
        public void MyUrls_should_be_routed_to_shorturl_list()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("~/MyUrls/5");
            var routeData = routes.GetRouteData(httpContext.Object);

            Assert.Equal("ShortUrl", routeData.Controller());
            Assert.Equal("List", routeData.Action());
            Assert.Equal(5, routeData.Value<int>("page"));
        }

        [Fact]
        public void P_should_be_routed_to_shorturl_preview()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("~/p/a1");
            var routeData = routes.GetRouteData(httpContext.Object);

            Assert.Equal("ShortUrl", routeData.Controller());
            Assert.Equal("Preview", routeData.Action());
            Assert.Equal("a1", routeData.Value<string>("alias"));
        }

        [Fact]
        public void Alphanumeric_value_should_be_routed_to_shorturl_visit()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("~/a1");
            var routeData = routes.GetRouteData(httpContext.Object);

            Assert.Equal("ShortUrl", routeData.Controller());
            Assert.Equal("Visit", routeData.Action());
            Assert.Equal("a1", routeData.Value<string>("alias"));
        }

        [Fact]
        public void ControlPanel_url_should_be_routed_to_control_panel_url()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("~/ControlPanel/Url/a1");
            var routeData = routes.GetRouteData(httpContext.Object);

            Assert.Equal("ControlPanel", routeData.Controller());
            Assert.Equal("Url", routeData.Action());
            Assert.Equal("a1", routeData.Value<string>("alias"));
        }

        [Fact]
        public void ControlPanel_user_should_be_routed_to_control_panel_user()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("~/ControlPanel/User/3");
            var routeData = routes.GetRouteData(httpContext.Object);

            Assert.Equal("ControlPanel", routeData.Controller());
            Assert.Equal("User", routeData.Action());
            Assert.Equal(3, routeData.Value<int>("id"));
        }

        [Fact]
        public void Api_create_should_be_routed_to_api_create()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("~/Api/Create");
            var routeData = routes.GetRouteData(httpContext.Object);

            Assert.Equal("Api", routeData.Controller());
            Assert.Equal("Create", routeData.Action());
        }
    }
}