namespace Shrinkr.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Microsoft.Practices.ServiceLocation;
    using MvcExtensions;

    using DomainObjects;
    using Infrastructure;
    using Repositories;

    public class AddReservedAliasFromRoutes : BootstrapperTask
    {
        private static readonly Type controllerType = typeof(Controller);

        public AddReservedAliasFromRoutes()
        {
            Order = DefaultOrder + 1;
        }

        protected override TaskContinuation ExecuteCore(IServiceLocator serviceLocator)
        {
            Check.Argument.IsNotNull(serviceLocator, "serviceLocator");

            RouteCollection routes = serviceLocator.GetInstance<RouteCollection>();
            IReservedAliasRepository repository = serviceLocator.GetInstance<IReservedAliasRepository>();
            IUnitOfWork unitOfWork = serviceLocator.GetInstance<IUnitOfWork>();

            IList<string> reservedAliases = new List<string>();

            Action<string> addIfNotExists = name =>
                                                {
                                                    if (!reservedAliases.Contains(name, StringComparer.OrdinalIgnoreCase) && !repository.IsMatching(name))
                                                    {
                                                        reservedAliases.Add(name);
                                                    }
                                                };

            foreach (Route route in routes.OfType<Route>())
            {
                string[] urlSegments = route.Url.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                if (urlSegments.Any())
                {
                    string firstSegment = urlSegments.First();

                    if (firstSegment.StartsWith("{", StringComparison.Ordinal) && firstSegment.EndsWith("}", StringComparison.Ordinal))
                    {
                        // Special Values, but we will only consider the the Controllers
                        if (firstSegment.Equals("{controller}", StringComparison.OrdinalIgnoreCase))
                        {
                            IEnumerable<Type> controllerTypes = GetType().Assembly.ConcreteTypes().Where(type => controllerType.IsAssignableFrom(type));

                            foreach (Type controller in controllerTypes)
                            {
                                string controllerName = controller.Name.Substring(0, controller.Name.Length - "Controller".Length);

                                addIfNotExists(controllerName);
                            }
                        }
                    }
                    else if (!firstSegment.StartsWith("{", StringComparison.Ordinal) && !firstSegment.EndsWith("}", StringComparison.Ordinal))
                    {
                        // Constant values
                        addIfNotExists(firstSegment);
                    }
                }
            }

            bool shouldCommit = false;

            foreach (string aliasName in reservedAliases.OrderBy(ra => ra))
            {
                repository.Add(new ReservedAlias { Name = aliasName });
                shouldCommit = true;
            }

            if (shouldCommit)
            {
                unitOfWork.Commit();
            }

            return TaskContinuation.Continue;
        }
    }
}