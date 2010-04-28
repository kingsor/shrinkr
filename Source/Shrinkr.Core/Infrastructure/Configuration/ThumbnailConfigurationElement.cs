namespace Shrinkr.Infrastructure
{
    using System.Configuration;
    using System.Diagnostics;

    public class ThumbnailConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("endPoint", IsRequired = true)]
        public string EndPoint
        {
            [DebuggerStepThrough]
            get
            {
                return (string) base["endPoint"];
            }

            [DebuggerStepThrough]
            set
            {
                base["endPoint"] = value;
            }
        }

        [ConfigurationProperty("apiKey", IsRequired = true)]
        public string ApiKey
        {
            [DebuggerStepThrough]
            get
            {
                return (string) base["apiKey"];
            }

            [DebuggerStepThrough]
            set
            {
                base["apiKey"] = value;
            }
        }
    }
}