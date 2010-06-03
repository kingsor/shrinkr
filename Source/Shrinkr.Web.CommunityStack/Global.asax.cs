namespace Shrinkr.Web.CommunityStack
{
    using System.Net;
    using System.Web;

    using MvcExtensions.Autofac;

    using Elmah;

    public class MvcApplication : AutofacMvcApplication
    {
        public void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            Check.Argument.IsNotNull(e, "e");

            HttpException exception = e.Exception.GetBaseException() as HttpException;

            if ((exception != null) && (exception.GetHttpCode() == (int)HttpStatusCode.NotFound))
            {
                e.Dismiss();
            }
        }
    }
}