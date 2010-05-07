namespace Shrinkr.Web
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;
    using Infrastructure;

    using Microsoft.Practices.ServiceLocation;
    using MvcExtensions;

    using Repositories;
    using Spark.Web.Mvc;
    

    public class RegisterViewEngine : BootstrapperTask
    {
        protected override TaskContinuation ExecuteCore(IServiceLocator serviceLocator)
        {
            Check.Argument.IsNotNull(serviceLocator, "serviceLocator");
            
            SparkEngineStarter.RegisterViewEngine();

            return TaskContinuation.Continue;
        }
    }
}