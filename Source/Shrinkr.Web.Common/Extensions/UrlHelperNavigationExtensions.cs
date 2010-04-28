namespace Shrinkr.Web
{
    using System.Web.Mvc;

    public static class UrlHelperNavigationExtensions
    {
        public static string ApplicationRoot(this UrlHelper instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            return instance.RequestContext.HttpContext.ApplicationRoot();
        }

        public static string Home(this UrlHelper instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            return instance.Content("~/");
        }

        public static string Create(this UrlHelper instance)
        {
            return RouteUrl(instance, "Default", null);
        }

        public static string Preview(this UrlHelper instance, string alias)
        {
            Check.Argument.IsNotNullOrEmpty(alias, "alias");

            return RouteUrl(instance, "Preview", new { alias });
        }

        public static string Visit(this UrlHelper instance, string alias)
        {
            Check.Argument.IsNotNullOrEmpty(alias, "alias");

            return RouteUrl(instance, "Visit", new { alias });
        }

        public static string Xrds(this UrlHelper instance)
        {
            return RouteUrl(instance, "Xrds", null);
        }

        public static string LogOn(this UrlHelper instance)
        {
            return RouteUrl(instance, "LogOn", null);
        }

        public static string List(this UrlHelper instance, int page)
        {
            Check.Argument.IsNotZeroOrNegative(page, "page");

            return RouteUrl(instance, "List", new { page });
        }

        public static string Profile(this UrlHelper instance)
        {
            return RouteUrl(instance, "Profile", null);
        }

        public static string LogOff(this UrlHelper instance)
        {
            return RouteUrl(instance, "LogOff", null);
        }

        public static string Summary(this UrlHelper instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            return instance.Action("Summary", "ControlPanel");
        }

        public static string Urls(this UrlHelper instance, int page)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotZeroOrNegative(page, "page");

            return instance.Action("Urls", "ControlPanel", new { page });
        }

        public static string Url(this UrlHelper instance, string alias)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotNullOrEmpty(alias, "alias");

            return instance.Action("Url", "ControlPanel", new { alias });
        }

        public static string Users(this UrlHelper instance, int page)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotZeroOrNegative(page, "page");

            return instance.Action("Users", "ControlPanel", new { page });
        }

        public static string User(this UrlHelper instance, long id)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotZeroOrNegative(id, "id");

            return instance.Action("User", "ControlPanel", new { id });
        }

        public static string BannedIPAddresses(this UrlHelper instance, int page)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotZeroOrNegative(page, "page");

            return instance.Action("BannedIPAddresses", "ControlPanel", new { page });
        }

        public static string BannedDomains(this UrlHelper instance, int page)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotZeroOrNegative(page, "page");

            return instance.Action("BannedDomains", "ControlPanel", new { page });
        }

        public static string ReservedAliases(this UrlHelper instance, int page)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotZeroOrNegative(page, "page");

            return instance.Action("ReservedAliases", "ControlPanel", new { page });
        }

        public static string BadWords(this UrlHelper instance, int page)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotZeroOrNegative(page, "page");

            return instance.Action("BadWords", "ControlPanel", new { page });
        }

        public static string ToAbsolute(this UrlHelper instance, string relativeUrl)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotNullOrEmpty(relativeUrl, "relativeUrl");

            return instance.RequestContext.HttpContext.ApplicationRoot() + relativeUrl;
        }

        private static string RouteUrl(UrlHelper helper, string routeName, object routeValues)
        {
            Check.Argument.IsNotNull(helper, "instance");

            return (routeValues == null) ? helper.RouteUrl(routeName) : helper.RouteUrl(routeName, routeValues);
        }
    }
}