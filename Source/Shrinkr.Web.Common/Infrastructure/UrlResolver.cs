namespace Shrinkr.Web
{
    using System.Diagnostics;
    using System.Web;
    using System.Web.Mvc;

    using Infrastructure;

    public class UrlResolver : IUrlResolver
    {
        private readonly UrlHelper urlHelper;

        public UrlResolver(HttpContextBase httpContext)
        {
            Check.Argument.IsNotNull(httpContext, "httpContext");

            urlHelper = httpContext.RequestContext().UrlHelper();
        }

        public string ApplicationRoot
        {
            [DebuggerStepThrough]
            get
            {
                return urlHelper.RequestContext.HttpContext.ApplicationRoot();
            }
        }

        public string Preview(string aliasName)
        {
            return urlHelper.Preview(aliasName);
        }

        public string Visit(string aliasName)
        {
            return urlHelper.Visit(aliasName);
        }

        public string Absolute(string relativeUrl)
        {
            return urlHelper.ToAbsolute(relativeUrl);
        }
    }
}