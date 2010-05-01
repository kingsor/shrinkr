namespace Shrinkr.Infrastructure
{
    using System.Configuration;
    using System.Diagnostics;

    public class SettingConfigurationSection : ConfigurationSection
    {
        private static string sectionName = "shrinkr";

        public static string SectionName
        {
            [DebuggerStepThrough]
            get
            {
                return sectionName;
            }

            [DebuggerStepThrough]
            set
            {
                Check.Argument.IsNotNullOrEmpty(value, "value");

                sectionName = value;
            }
        }

        [ConfigurationProperty("redirectPermanently", DefaultValue = true)]
        public bool RedirectPermanently
        {
            [DebuggerStepThrough]
            get
            {
                return (bool) this["redirectPermanently"];
            }

            [DebuggerStepThrough]
            set
            {
                this["redirectPermanently"] = value;
            }
        }

        [ConfigurationProperty("urlPerPage", DefaultValue = 5)]
        public int UrlPerPage
        {
            [DebuggerStepThrough]
            get
            {
                return (int) this["urlPerPage"];
            }

            [DebuggerStepThrough]
            set
            {
                this["urlPerPage"] = value;
            }
        }

        [ConfigurationProperty("baseType", DefaultValue = BaseType.BaseThirtySix)]
        public BaseType BaseType
        {
            [DebuggerStepThrough]
            get
            {
                return (BaseType)base["baseType"];
            }

            [DebuggerStepThrough]
            set
            {
                base["baseType"] = value;
            }
        }

        [ConfigurationProperty("api")]
        public ApiConfigurationElement Api
        {
            [DebuggerStepThrough]
            get
            {
                return base["api"] as ApiConfigurationElement ?? new ApiConfigurationElement();
            }
        }

        [ConfigurationProperty("thumbnail", IsRequired = true)]
        public ThumbnailConfigurationElement Thumbnail
        {
            [DebuggerStepThrough]
            get
            {
                return base["thumbnail"] as ThumbnailConfigurationElement;
            }
        }

        [ConfigurationProperty("google", IsRequired = true)]
        public GoogleSafeBrowsingConfigurationElement Google
        {
            [DebuggerStepThrough]
            get
            {
                return base["google"] as GoogleSafeBrowsingConfigurationElement;
            }
        }

        [ConfigurationProperty("twitter")]
        public TwitterConfigurationElement Twitter
        {
            [DebuggerStepThrough]
            get
            {
                return base["twitter"] as TwitterConfigurationElement;
            }
        }

        [ConfigurationProperty("users")]
        public DefaultUserConfigurationElementCollection DefaultUsers
        {
            [DebuggerStepThrough]
            get
            {
                return this["users"] as DefaultUserConfigurationElementCollection ?? new DefaultUserConfigurationElementCollection();
            }
        }
    }
}