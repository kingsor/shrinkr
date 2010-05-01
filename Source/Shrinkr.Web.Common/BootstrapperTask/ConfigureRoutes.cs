namespace Shrinkr.Web
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    using MvcExtensions;

    public class ConfigureRoutes : RegisterRoutesBase
    {
        protected override void Register(RouteCollection routes)
        {
            Func<IRouteConstraint> idConstraint = () => new PositiveLongConstraint();
            Func<IRouteConstraint> pageConstraint = () => new PositiveIntegerConstraint(true);
            Func<IRouteConstraint> aliasConstraint = () => new RegexConstraint(@"^[a-zA-Z0-9]+$");

            Check.Argument.IsNotNull(routes, "routes");

            routes.Clear();

            // Turns off the unnecessary file exists check
            routes.RouteExistingFiles = true;

            // Ignore axd files
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Ignore known static files
            routes.IgnoreRoute("{file}.txt");
            routes.IgnoreRoute("{file}.htm");
            routes.IgnoreRoute("{file}.html");
            routes.IgnoreRoute("{file}.xml");

            // Ignore the assets directory which contains css, images and js
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");

            routes.IgnoreRoute("{*favicon}", new { favicon = new RegexConstraint(@"(.*/)?favicon.([iI][cC][oO]|[gG][iI][fF])(/.*)?") });

            routes.MapRoute("Xrds", "Xrds", new { controller = "Authentication", action = "Xrds" });
            routes.MapRoute("LogOn", "LogOn", new { controller = "Authentication", action = "LogOn" });
            routes.MapRoute("LogOff", "LogOff", new { controller = "Authentication", action = "LogOff" });
            routes.MapRoute("Profile", "MyProfile", new { controller = "User", action = "Profile" });
            routes.MapRoute("GenerateKey", "GenerateKey", new { controller = "User", action = "GenerateKey" });

            routes.MapRoute("Url", "ControlPanel/Url/{alias}", new { controller = "ControlPanel", action = "Url" }, new { alias = aliasConstraint() });
            routes.MapRoute("User", "ControlPanel/User/{id}", new { controller = "ControlPanel", action = "User" }, new { id = idConstraint() });
            routes.MapRoute("ControlPanel", "ControlPanel/{action}/{page}", new { controller = "ControlPanel", action = "Summary", page = 1 }, new { page = pageConstraint() });
            routes.MapRoute("Api", "Api/{action}", new { controller = "Api", action = "Create" });

            routes.MapRoute("List", "MyUrls/{page}", new { controller = "ShortUrl", action = "List", page = 1 }, new { page = pageConstraint() });
            routes.MapRoute("Preview", "p/{alias}", new { controller = "ShortUrl", action = "Preview" }, new { alias = aliasConstraint() });
            routes.MapRoute("Visit", "{alias}", new { controller = "ShortUrl", action = "Visit" }, new { alias = aliasConstraint() });

            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "ShortUrl", action = "Create", id = string.Empty });
        }
    }
}