namespace Shrinkr.Web
{
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure;

    using Microsoft.Practices.ServiceLocation;
    using MvcExtensions;

    public class ConfigureBackgroundServices : BootstrapperTask
    {
        private IEnumerable<IBackgroundService> backgroundServices;

        protected override TaskContinuation ExecuteCore(IServiceLocator serviceLocator)
        {
            Check.Argument.IsNotNull(serviceLocator, "serviceLocator");

            backgroundServices = serviceLocator.GetAllInstances<IBackgroundService>().ToList();

            backgroundServices.Each(service => service.Start());

            return TaskContinuation.Continue;
        }

        protected override void DisposeCore()
        {
            backgroundServices.Each(service => service.Stop());
        }
    }
}