namespace Shrinkr.Infrastructure
{
    public abstract class BackgroundServiceBase : IBackgroundService
    {
        protected BackgroundServiceBase(IEventAggregator eventAggregator)
        {
            Check.Argument.IsNotNull(eventAggregator, "eventAggregator");

            EventAggregator = eventAggregator;
        }

        public abstract string Name
        {
            get;
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        protected IEventAggregator EventAggregator
        {
            get;
            private set;
        }

        public void Start()
        {
            if (!IsRunning)
            {
                OnStart();
                IsRunning = true;
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                OnStop();
                IsRunning = false;
            }
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnStop()
        {
        }
    }
}