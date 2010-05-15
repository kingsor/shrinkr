namespace Shrinkr.Infrastructure
{
    using System.Configuration;
    using System.Diagnostics;

    public class GoogleSafeBrowsingConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("apiKey", IsRequired = true)]
        public string ApiKey
        {
            [DebuggerStepThrough]
            get
            {
                return (string)base["apiKey"];
            }

            [DebuggerStepThrough]
            set
            {
                base["apiKey"] = value;
            }
        }

        [ConfigurationProperty("endpoint", IsRequired = true)]
        public string Endpoint
        {
            [DebuggerStepThrough]
            get
            {
                return (string)base["endpoint"];
            }

            [DebuggerStepThrough]
            set
            {
                base["endpoint"] = value;
            }
        }

        [ConfigurationProperty("phishingFile", DefaultValue = "~/App_Data/GoogleSafeBrowsing/phishing.xml")]
        public string PhishingFile
        {
            [DebuggerStepThrough]
            get
            {
                return (string)base["phishingFile"];
            }

            [DebuggerStepThrough]
            set
            {
                base["phishingFile"] = value;
            }
        }

        [ConfigurationProperty("malwareFile", DefaultValue = "~/App_Data/GoogleSafeBrowsing/malware.xml")]
        public string MalwareFile
        {
            [DebuggerStepThrough]
            get
            {
                return (string)base["malwareFile"];
            }

            [DebuggerStepThrough]
            set
            {
                base["malwareFile"] = value;
            }
        }
    }
}