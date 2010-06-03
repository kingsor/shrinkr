namespace Shrinkr.Web
{
    using System.Web.Mvc;

    [Bind(Exclude = "ResponseFormat, IPAddress")]
    public abstract class ApiCommand
    {
        public string ApiKey
        {
            get;
            set;
        }

        public ApiResponseFormat ResponseFormat
        {
            get;
            set;
        }

        public string IPAddress
        {
            get;
            set;
        }
    }
}