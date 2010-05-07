namespace Shrinkr.Infrastructure
{
    using System;
    using System.Diagnostics;
    using System.Timers;

    public class GoogleSafeBrowsingDataUpdater : BackgroundServiceBase, IDisposable
    {
        private readonly IGoogleSafeBrowsing google;
        private Timer timer;

        private bool isDisposed;

        public GoogleSafeBrowsingDataUpdater(IEventAggregator eventAggregator, IGoogleSafeBrowsing google) : base(eventAggregator)
        {
            Check.Argument.IsNotNull(google, "google");

            this.google = google;
        }

        [DebuggerStepThrough]
        ~GoogleSafeBrowsingDataUpdater()
        {
            Dispose(false);
        }

        public override string Name
        {
            get
            {
                return "Google Safe Browsing Data Updater";
            }
        }

        [DebuggerStepThrough]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [DebuggerStepThrough]
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                if (timer != null)
                {
                    timer.Stop();
                    timer.Dispose();
                }
            }

            isDisposed = true;
        }

        protected override void OnStart()
        {
            google.Update();

            timer = new Timer(1000 * 60 * 30);
            timer.Elapsed += OnElapsed;
            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Elapsed -= OnElapsed;
            timer.Stop();
            timer.Dispose();
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            google.Update();
        }
    }
}