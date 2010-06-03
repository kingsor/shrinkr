namespace Shrinkr.Web
{
    using System;
    using System.Net;
    using System.Web;

    using MvcExtensions;

    public class RemoveWww : PerRequestTask
    {
        private readonly HttpContextBase httpContext;

        public RemoveWww(HttpContextBase httpContext)
        {
            Invariant.IsNotNull(httpContext, "httpContext");

            this.httpContext = httpContext;
        }

        public override TaskContinuation Execute()
        {
            const string Prefix = "http://www.";

            string url = httpContext.Request.Url.ToString();

            bool startsWith3W = url.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase);
            bool shouldContinue = true;

            if (startsWith3W)
            {
                string newUrl = "http://" + url.Substring(Prefix.Length);

                HttpResponseBase response = httpContext.Response;

                response.StatusCode = (int)HttpStatusCode.MovedPermanently;
                response.Status = "301 Moved Permanently";
                response.RedirectLocation = newUrl;
                response.SuppressContent = true;
                response.End();
                shouldContinue = false;
            }

            return shouldContinue ? TaskContinuation.Continue : TaskContinuation.Break;
        }
    }
}