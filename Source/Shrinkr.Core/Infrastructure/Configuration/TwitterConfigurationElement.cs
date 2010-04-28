namespace Shrinkr.Infrastructure
{
    using System.Configuration;
    using System.Diagnostics;

    public class TwitterConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("userName", IsRequired = true)]
        public string UserName
        {
            [DebuggerStepThrough]
            get
            {
                return (string)base["userName"];
            }

            [DebuggerStepThrough]
            set
            {
                base["userName"] = value;
            }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            [DebuggerStepThrough]
            get
            {
                return (string) base["password"];
            }

            [DebuggerStepThrough]
            set
            {
                base["password"] = value;
            }
        }

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

        [ConfigurationProperty("maximumMessageLength", IsRequired = true)]
        public int MaximumMessageLength
        {
            [DebuggerStepThrough]
            get
            {
                return (int) base["maximumMessageLength"];
            }

            [DebuggerStepThrough]
            set
            {
                base["maximumMessageLength"] = value;
            }
        }

        [ConfigurationProperty("messageTemplate", IsRequired = true)]
        public string MessageTemplate
        {
            [DebuggerStepThrough]
            get
            {
                return (string) base["messageTemplate"];
            }

            [DebuggerStepThrough]
            set
            {
                base["messageTemplate"] = value;
            }
        }

        [ConfigurationProperty("recipients", IsRequired = true)]
        public string Recipients
        {
            [DebuggerStepThrough]
            get
            {
                return (string)base["recipients"];
            }

            [DebuggerStepThrough]
            set
            {
                base["recipients"] = value;
            }
        }
    }
}