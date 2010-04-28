namespace Shrinkr.Infrastructure
{
    public class GoogleSafeBrowsingSettings
    {
        public GoogleSafeBrowsingSettings(string apiKey, string endPoint, string phishingFile, string malwareFile)
        {
            Check.Argument.IsNotNullOrEmpty(apiKey, "apiKey");
            Check.Argument.IsNotNullOrEmpty(endPoint, "endPoint");
            Check.Argument.IsNotNullOrEmpty(phishingFile, "phishingFile");
            Check.Argument.IsNotNullOrEmpty(malwareFile, "malwareFile");

            ApiKey = apiKey;
            EndPoint = endPoint;
            PhishingFile = phishingFile;
            MalwareFile = malwareFile;
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

        public string PhishingFile
        {
            get;
            private set;
        }

        public string MalwareFile
        {
            get;
            private set;
        }
    }
}