namespace Shrinkr.Infrastructure
{
    using System.Configuration;
    using System.Diagnostics;

    using DomainObjects;

    public class DefaultUserConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            [DebuggerStepThrough]
            get
            {
                return (string)this["name"];
            }

            [DebuggerStepThrough]
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("email")]
        public string Email
        {
            [DebuggerStepThrough]
            get
            {
                return (string)this["email"];
            }

            [DebuggerStepThrough]
            set
            {
                this["email"] = value;
            }
        }

        [ConfigurationProperty("role", DefaultValue = Role.Administrator)]
        public Role Role
        {
            [DebuggerStepThrough]
            get
            {
                return (Role)this["role"];
            }

            [DebuggerStepThrough]
            set
            {
                this["role"] = value;
            }
        }
    }
}