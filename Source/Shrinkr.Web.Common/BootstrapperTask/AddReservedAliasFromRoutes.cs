﻿namespace Shrinkr.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    using DomainObjects;
    using Infrastructure;
    using MvcExtensions;
    using Repositories;

    public class AddReservedAliasFromRoutes : BootstrapperTask
    {
        private static readonly Type controllerType = typeof(Controller);

        private readonly RouteCollection routes;
        private readonly IReservedAliasRepository reservedAliasRepository;
        private readonly IUnitOfWork unitOfWork;

        public AddReservedAliasFromRoutes(RouteCollection routes, IReservedAliasRepository reservedAliasRepository, IUnitOfWork unitOfWork)
        {
            Check.Argument.IsNotNull(routes, "routes");
            Check.Argument.IsNotNull(reservedAliasRepository, "reservedAliasRepository");
            Check.Argument.IsNotNull(unitOfWork, "unitOfWork");

            this.routes = routes;
            this.reservedAliasRepository = reservedAliasRepository;
            this.unitOfWork = unitOfWork;

            Order = DefaultOrder + 1;
        }

        public override TaskContinuation Execute()
        {
            IList<string> reservedAliases = new List<string>();

            Action<string> addIfNotExists = name =>
            {
                if (!reservedAliases.Contains(name, StringComparer.OrdinalIgnoreCase) && !reservedAliasRepository.IsMatching(name))
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
                reservedAliasRepository.Add(new ReservedAlias { Name = aliasName });
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