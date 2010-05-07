namespace Shrinkr.Infrastructure
{
    public class GoogleSafeBrowsingSettings
    {
        public GoogleSafeBrowsingSettings(string apiKey, string endpoint, string phishingFile, string malwareFile)
        {
            Check.Argument.IsNotNullOrEmpty(apiKey, "apiKey");
            Check.Argument.IsNotNullOrEmpty(endpoint, "endpoint");
            Check.Argument.IsNotNullOrEmpty(phishingFile, "phishingFile");
            Check.Argument.IsNotNullOrEmpty(malwareFile, "malwareFile");

            ApiKey = apiKey;
            Endpoint = endpoint;
            PhishingFile = phishingFile;
            MalwareFile = malwareFile;
        }

        public string ApiKey
        {
            get;
            private set;
        }

        public string Endpoint
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