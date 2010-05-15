namespace Shrinkr.Web
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using MvcExtensions;

    [BindingType(typeof(ApiCommand), true)]
    public class ApiCommandBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Check.Argument.IsNotNull(controllerContext, "controllerContext");

            ApiCommand command = (ApiCommand)base.BindModel(controllerContext, bindingContext);

            HttpContextBase httpContext = controllerContext.HttpContext;
            HttpRequestBase httpRequest = httpContext.Request;

            command.IPAddress = httpRequest.UserHostAddress;

            string format = null;

            // First we will check whether format is explicitly specified
            ValueProviderResult providerResult = controllerContext.Controller.ValueProvider.GetValue("format");

            if (providerResult != null)
            {
                format = (string)providerResult.ConvertTo(typeof(string), Culture.Current);
            }

            ApiResponseFormat responseFormat = ApiResponseFormat.Text;

            if (string.IsNullOrWhiteSpace(format) || !Enum.TryParse(format, true, out responseFormat))
            {
                // No format exist, try the AcceptTypes
                if ((httpRequest.AcceptTypes != null) && (httpRequest.AcceptTypes.Length > 0))
                {
                    string firstAcceptType = QValueSorter.Sort(httpRequest.AcceptTypes).FirstOrDefault() ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(firstAcceptType))
                    {
                        if (firstAcceptType.Equals("application/json", StringComparison.OrdinalIgnoreCase) || firstAcceptType.Equals("text/json", StringComparison.OrdinalIgnoreCase))
                        {
                            responseFormat = ApiResponseFormat.Json;
                        }
                        else if (firstAcceptType.Equals("application/xml", StringComparison.OrdinalIgnoreCase) || firstAcceptType.Equals("text/xml", StringComparison.OrdinalIgnoreCase))
                        {
                            responseFormat = ApiResponseFormat.Xml;
                        }
                    }
                }
            }

            command.ResponseFormat = responseFormat;

            return command;
        }
    }
}