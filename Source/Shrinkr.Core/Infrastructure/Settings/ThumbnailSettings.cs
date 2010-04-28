namespace Shrinkr.Infrastructure
{
    public class ThumbnailSettings
    {
        public ThumbnailSettings(string apiKey, string endPoint)
        {
            Check.Argument.IsNotNullOrEmpty(apiKey, "apiKey");
            Check.Argument.IsNotNullOrEmpty(endPoint, "endPoint");

            ApiKey = apiKey;
            EndPoint = endPoint;
        }

        public string ApiKey
        {
            get;
            private set;
        }

        public string EndPoint
        {
            get;
            private set;
        }
    }
}