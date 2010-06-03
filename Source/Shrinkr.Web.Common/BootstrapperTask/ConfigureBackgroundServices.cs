namespace Shrinkr.Web
{
    using System.Collections.Generic;

    using Infrastructure;

    using MvcExtensions;

    public class ConfigureBackgroundServices : BootstrapperTask
    {
        private readonly IEnumerable<IBackgroundService> backgroundServices;

        public ConfigureBackgroundServices(IBackgroundService[] backgroundServices)
        {
            Check.Argument.IsNotNull(backgroundServices, "backgroundServices");

            this.backgroundServices = backgroundServices;
        }

        public override TaskContinuation Execute()
        {
            backgroundServices.Each(service => service.Start());

            return TaskContinuation.Continue;
        }

        protected override void DisposeCore()
        {
            backgroundServices.Each(service => service.Stop());
        }
    }
}