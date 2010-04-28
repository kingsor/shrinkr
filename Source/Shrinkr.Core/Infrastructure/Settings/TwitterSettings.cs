namespace Shrinkr.Infrastructure
{
    using System.Collections.Generic;

    public class TwitterSettings
    {
        public TwitterSettings(string userName, string password, string endPoint, string messageTemplate, int maximumMessageLength, IEnumerable<string> recipients)
        {
            Check.Argument.IsNotNullOrEmpty(userName, "userName");
            Check.Argument.IsNotNullOrEmpty(password, "password");
            Check.Argument.IsNotNullOrEmpty(endPoint, "endPoint");
            Check.Argument.IsNotNullOrEmpty(messageTemplate, "messageTemplate");
            Check.Argument.IsNotZeroOrNegative(maximumMessageLength, "maximumMessageLength");
            Check.Argument.IsNotNull(recipients, "recipients");

            UserName = userName;
            Password = password;
            EndPoint = endPoint;
            MessageTemplate = messageTemplate;
            MaximumMessageLength = maximumMessageLength;
            Recipients = recipients;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string EndPoint
        {
            get;
            set;
        }

        public string MessageTemplate
        {
            get;
            set;
        }

        public int MaximumMessageLength
        {
            get;
            private set;
        }

        public IEnumerable<string> Recipients
        {
            get;
            private set;
        }
    }
}