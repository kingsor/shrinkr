namespace Shrinkr.Infrastructure
{
    using System.Configuration;
    using System.Diagnostics;

    public class ApiConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("allowed")]
        public bool Allowed
        {
            [DebuggerStepThrough]
            get
            {
                return (bool)base["allowed"];
            }

            [DebuggerStepThrough]
            set
            {
                base["allowed"] = value;
            }
        }

        [ConfigurationProperty("dailyLimit")]
        public int DailyLimit
        {
            [DebuggerStepThrough]
            get
            {
                return (int)base["dailyLimit"];
            }

            [DebuggerStepThrough]
            set
            {
                base["dailyLimit"] = value;
            }
        }
    }
}