namespace Shrinkr.Web
{
    using System.Web.Mvc;

    using Extensions;

    public static class UrlHelperAssetExtensions
    {
        public static string OpenIdIcon(this UrlHelper instance, string icon)
        {
            Check.Argument.IsNotNull(instance, "helper");
            Check.Argument.IsNotNullOrEmpty(icon, "icon");

            return instance.Content("~/Content/images/openid/{0}.png".FormatWith(icon));
        }

        public static string InputValidationErrorIcon(this UrlHelper instance)
        {
            Check.Argument.IsNotNull(instance, "helper");

            return instance.Content("~/Content/images/forms/exclamation.png");
        }
    }
}