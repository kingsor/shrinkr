namespace Shrinkr.Infrastructure
{
    using System.Collections.Generic;

    using DomainObjects;

    public class TwitterNotificationService : BackgroundServiceBase
    {
        private readonly Settings settings;
        private readonly IHttp http;

        private SubscriptionToken eventToken;

        public TwitterNotificationService(Settings settings, IEventAggregator eventAggregator, IHttp http) : base(eventAggregator)
        {
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotNull(http, "http");

            this.settings = settings;
            this.http = http;
        }

        public override string Name
        {
            get
            {
                return "Twitter Notification";
            }
        }

        protected override void OnStart()
        {
            if (settings.Twitter != null)
            {
                eventToken = EventAggregator.GetEvent<PossibleSpamDetectedEvent>().Subscribe(OnSpamDetected);
            }
        }

        protected override void OnStop()
        {
            if (settings.Twitter != null)
            {
                EventAggregator.GetEvent<PossibleSpamDetectedEvent>().Unsubscribe(eventToken);
            }
        }

        private void OnSpamDetected(EventArgs<Alias> eventArgs)
        {
            TwitterSettings twitter = settings.Twitter;

            if (twitter != null)
            {
                Alias alias = eventArgs.Value;

                string template = twitter.MessageTemplate;

                string message = template.Replace("{alias}", alias.Name).Replace("{title}", alias.ShortUrl.Title);

                if (message.Length > twitter.MaximumMessageLength)
                {
                    message = message.Substring(0, twitter.MaximumMessageLength);
                }

                foreach (string recipient in twitter.Recipients)
                {
                    http.PostAsync(twitter.EndPoint, twitter.UserName, twitter.Password, new Dictionary<string, string> { { "user", recipient }, { "text", message } });
                }
            }
        }
    }
}