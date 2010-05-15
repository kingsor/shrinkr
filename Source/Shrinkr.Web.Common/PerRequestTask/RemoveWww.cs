namespace Shrinkr.Web
{
    using System;
    using System.Net;
    using System.Web;

    using MvcExtensions;

    public class RemoveWww : PerRequestTask
    {
        protected override TaskContinuation ExecuteCore(PerRequestExecutionContext executionContext)
        {
            const string Prefix = "http://www.";

            Check.Argument.IsNotNull(executionContext, "executionContext");

            HttpContextBase httpContext = executionContext.HttpContext;

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