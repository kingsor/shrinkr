namespace Shrinkr.Web.UnitTests
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Extensions;

    using Moq;

    public static class MvcTestHelper
    {
        public const string AppPathModifier = "http://shrinkr.com";

        public static Mock<HttpContextBase> MockHttpContext(this Controller controller)
        {
            return MockHttpContext(controller, null, null, null);
        }

        public static Mock<HttpContextBase> MockHttpContext(this Controller controller, string appPath, string requestPath, string httpMethod)
        {
            var httpContext = CreateHttpContext(appPath, requestPath, httpMethod);

            controller.ControllerContext = new ControllerContext(httpContext.Object, new RouteData(), controller);
            controller.Url = new UrlHelper(controller.ControllerContext.RequestContext);

            return httpContext;
        }

        public static Mock<HttpContextBase> CreateHttpContext()
        {
            return CreateHttpContext(null, null, null);
        }

        public static Mock<HttpContextBase> CreateHttpContext(string requestPath)
        {
            return CreateHttpContext(null, requestPath, null);
        }

        public static Mock<HttpContextBase> CreateHttpContext(string appPath, string requestPath, string httpMethod)
        {
            var httpContext = new Mock<HttpContextBase>();

            if (!string.IsNullOrWhiteSpace(appPath))
            {
                httpContext.SetupGet(c => c.Request.ApplicationPath).Returns(appPath);
            }

            if (!string.IsNullOrWhiteSpace(requestPath))
            {
                httpContext.SetupGet(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns(requestPath);
                httpContext.SetupGet(c => c.Request.Url).Returns(new Uri("{0}{1}".FormatWith(AppPathModifier, requestPath.Replace("~", string.Empty))));
            }

            httpContext.SetupGet(c => c.Request.PathInfo).Returns(string.Empty);

            if (!string.IsNullOrWhiteSpace(httpMethod))
            {
                httpContext.SetupGet(c => c.Request.HttpMethod).Returns(httpMethod);
            }

            httpContext.Setup(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => r.Contains(AppPathModifier) ? r : AppPathModifier + r);

            return httpContext;
        }

        public static string Controller(this RouteValueDictionary instance)
        {
            return instance["controller"] as string;
        }

        public static string Action(this RouteValueDictionary instance)
        {
            return instance["action"] as string;
        }

        public static T Value<T>(this RouteValueDictionary instance, string key)
        {
            return (T) Convert.ChangeType(instance[key], typeof(T));
        }

        public static string Controller(this RouteData instance)
        {
            return instance.Values.Controller();
        }

        public static string Action(this RouteData instance)
        {
            return instance.Values.Action();
        }

        public static T Value<T>(this RouteData instance, string key)
        {
            return instance.Values.Value<T>(key);
        }
    }
}