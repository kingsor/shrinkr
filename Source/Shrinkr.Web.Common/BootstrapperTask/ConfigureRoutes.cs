namespace Shrinkr.Web
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    using MvcExtensions;

    public class ConfigureRoutes : RegisterRoutesBase
    {
        public ConfigureRoutes(RouteCollection routes) : base(routes)
        {
        }

        protected override void Register()
        {
            Func<IRouteConstraint> idConstraint = () => new PositiveLongConstraint();
            Func<IRouteConstraint> pageConstraint = () => new PositiveIntegerConstraint(true);
            Func<IRouteConstraint> aliasConstraint = () => new RegexConstraint(@"^[a-zA-Z0-9]+$");

            Routes.Clear();

            // Turns off the unnecessary file exists check
            Routes.RouteExistingFiles = true;

            // Ignore axd files
            Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Ignore known static files
            Routes.IgnoreRoute("{file}.txt");
            Routes.IgnoreRoute("{file}.htm");
            Routes.IgnoreRoute("{file}.html");
            Routes.IgnoreRoute("{file}.xml");

            // Ignore the assets directory which contains css, images and js
            Routes.IgnoreRoute("Content/{*pathInfo}");
            Routes.IgnoreRoute("Scripts/{*pathInfo}");

            Routes.IgnoreRoute("{*favicon}", new { favicon = new RegexConstraint(@"(.*/)?favicon.([iI][cC][oO]|[gG][iI][fF])(/.*)?") });

            Routes.MapRoute("Xrds", "Xrds", new { controller = "Authentication", action = "Xrds" });
            Routes.MapRoute("LogOn", "LogOn", new { controller = "Authentication", action = "LogOn" });
            Routes.MapRoute("LogOff", "LogOff", new { controller = "Authentication", action = "LogOff" });
            Routes.MapRoute("Profile", "MyProfile", new { controller = "User", action = "Profile" });
            Routes.MapRoute("GenerateKey", "GenerateKey", new { controller = "User", action = "GenerateKey" });

            Routes.MapRoute("Url", "ControlPanel/Url/{alias}", new { controller = "ControlPanel", action = "Url" }, new { alias = aliasConstraint() });
            Routes.MapRoute("User", "ControlPanel/User/{id}", new { controller = "ControlPanel", action = "User" }, new { id = idConstraint() });
            Routes.MapRoute("ControlPanel", "ControlPanel/{action}/{page}", new { controller = "ControlPanel", action = "Summary", page = 1 }, new { page = pageConstraint() });
            Routes.MapRoute("Api", "Api/{action}", new { controller = "Api", action = "Create" });

            Routes.MapRoute("List", "MyUrls/{page}", new { controller = "ShortUrl", action = "List", page = 1 }, new { page = pageConstraint() });
            Routes.MapRoute("Preview", "p/{alias}", new { controller = "ShortUrl", action = "Preview" }, new { alias = aliasConstraint() });
            Routes.MapRoute("Visit", "{alias}", new { controller = "ShortUrl", action = "Visit" }, new { alias = aliasConstraint() });

            Routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "ShortUrl", action = "Create", id = string.Empty });
        }
    }
}