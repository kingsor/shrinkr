namespace Shrinkr.Web
{
    using System.Web;
    using System.Web.Mvc;

    using MvcExtensions;

    [BindingType(typeof(UserCommand), true)]
    public class UserCommandBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Check.Argument.IsNotNull(controllerContext, "controllerContext");

            UserCommand userCommand = (UserCommand)base.BindModel(controllerContext, bindingContext);

            HttpContextBase httpContext = controllerContext.HttpContext;

            userCommand.UserName = (httpContext.User != null) &&
                                   (httpContext.User.Identity != null) &&
                                   httpContext.User.Identity.IsAuthenticated ?
                                   httpContext.User.Identity.Name :
                                   null;

            userCommand.IPAddress = httpContext.Request.UserHostAddress;

            HttpBrowserCapabilitiesBase browser = httpContext.Request.Browser;

            if (browser != null)
            {
                userCommand.Browser = (browser.Browser + " " + browser.Version).Trim();
            }

            userCommand.Referrer = (httpContext.Request.UrlReferrer != null) ? httpContext.Request.UrlReferrer.ToString() : null;

            return userCommand;
        }
    }
}