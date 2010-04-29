namespace Shrinkr.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Common;
    using System.Linq;
    using System.Web;

    using Microsoft.Practices.Unity;

    using MvcExtensions;
    using MvcExtensions.Unity;

    using DomainObjects;
    using Infrastructure;
    using Infrastructure.EntityFramework;
    using Repositories;
    using Services;

    public class RegisterServices : IModule
    {
        private static readonly Func<LifetimeManager> transient = () => new TransientLifetimeManager();
        private static readonly Func<LifetimeManager> perRequest = () => new PerRequestLifetimeManager();
        private static readonly Func<LifetimeManager> singleton = () => new ContainerControlledLifetimeManager();

        public void Load(IUnityContainer container)
        {
            Check.Argument.IsNotNull(container, "container");

            container.RegisterType<IHttp, Http>(singleton())
                     .RegisterType<IThumbnail, PageGlimpseThumbnail>(singleton())
                     .RegisterType<IEventAggregator, EventAggregator>(singleton())
                     .RegisterType<ICacheManager, CacheManager>(singleton(), new InjectionConstructor(HttpRuntime.Cache))
                     .RegisterType<IExternalContentService, ExternalContentService>(singleton())
                     .RegisterType<IGoogleSafeBrowsing, GoogleSafeBrowsing>(singleton())
                     .RegisterType<IUrlResolver, UrlResolver>(perRequest())
                     .RegisterType<IUnitOfWork, UnitOfWork>(perRequest())
                     .RegisterType<IUserService, UserService>(perRequest())
                     .RegisterType<IShortUrlService, ShortUrlService>(perRequest())
                     .RegisterType<IAdministrativeService, AdministrativeService>(perRequest())
                     .RegisterType<IOpenIdRelyingParty, OpenIdRelyingParty>(transient())
                     .RegisterType<IFormsAuthentication, FormsAuthentication>(singleton())
                     .RegisterType<ICookie, Cookie>(transient());

            IBuildManager buildManager = container.Resolve<IBuildManager>();

            RegisterRepositories(buildManager, container);
            RegisterSpamDetectors(buildManager, container);
            RegisterBackgroundServices(buildManager, container);

            Settings settings = CreateSettings(container.Resolve<HttpContextBase>());

            container.RegisterInstance(settings)
                     .RegisterType<IBaseX, BaseX>(singleton(), new InjectionConstructor(settings.BaseType));

            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["Shrinkr"];

            string providerName = connectionStringSettings.ProviderName;

            DbProviderFactory databaseProviderFactory = DbProviderFactories.GetFactory(providerName);
            container.RegisterInstance(databaseProviderFactory);

            string connectionString = connectionStringSettings.ConnectionString;
            bool? useCompliledQuery = null;
            bool temp;

            if (bool.TryParse(ConfigurationManager.AppSettings["useCompliedQuery"], out temp))
            {
                useCompliledQuery = temp;
            }

            container.RegisterType<IDatabaseFactory, DatabaseFactory>(perRequest(), new InjectionConstructor(typeof(DbProviderFactory), connectionString))
                     .RegisterType<IQueryFactory, QueryFactory>(singleton(), new InjectionConstructor(settings.BaseType == BaseType.BaseSixtyTwo, useCompliledQuery ?? true));
        }

        private static void RegisterRepositories(IBuildManager buildManager, IUnityContainer container)
        {
            Type genericRepositoryType = typeof(IRepository<>);

            IEnumerable<Type> repositoryContractTypes = buildManager.PublicTypes.Where(type => (type != null) && type.IsInterface && type.GetInterfaces().Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition().Equals(genericRepositoryType))).ToList();

            foreach (Type repositoryImplementationType in buildManager.ConcreteTypes.Where(implementationType => repositoryContractTypes.Any(contractType => contractType.IsAssignableFrom(implementationType))))
            {
                foreach (Type repositoryInterfaceType in repositoryImplementationType.GetInterfaces())
                {
                    container.RegisterType(repositoryInterfaceType, repositoryImplementationType, perRequest());
                }
            }
        }

        private static void RegisterSpamDetectors(IBuildManager buildManager, IUnityContainer container)
        {
            Type spamDetectorInterfaceType = typeof(ISpamDetector);

            foreach (Type spamDetectorType in buildManager.ConcreteTypes.Where(spamDetectorInterfaceType.IsAssignableFrom))
            {
                container.RegisterType(spamDetectorInterfaceType, spamDetectorType, spamDetectorType.FullName, perRequest());
            }
        }

        private static void RegisterBackgroundServices(IBuildManager buildManager, IUnityContainer container)
        {
            Type backgroundServiceInterfaceType = typeof(IBackgroundService);

            foreach (Type backgroundServiceType in buildManager.ConcreteTypes.Where(backgroundServiceInterfaceType.IsAssignableFrom))
            {
                container.RegisterType(backgroundServiceInterfaceType, backgroundServiceType, backgroundServiceType.FullName, singleton());
            }
        }

        private static Settings CreateSettings(HttpContextBase httpContext)
        {
            SettingConfigurationSection section = (SettingConfigurationSection) ConfigurationManager.GetSection(SettingConfigurationSection.SectionName);

            ApiSettings api = new ApiSettings(section.Api.Allowed, section.Api.DailyLimit);

            ThumbnailSettings thumbnail = new ThumbnailSettings(section.Thumbnail.ApiKey, section.Thumbnail.Endpoint);

            GoogleSafeBrowsingSettings google = new GoogleSafeBrowsingSettings(section.Google.ApiKey, section.Google.Endpoint, httpContext.Server.MapPath(section.Google.PhishingFile), httpContext.Server.MapPath(section.Google.MalwareFile));

            TwitterSettings twitter = null;

            if (section.Twitter != null)
            {
                twitter = new TwitterSettings(section.Twitter.UserName, section.Twitter.Password, section.Twitter.Endpoint, section.Twitter.MessageTemplate, section.Twitter.MaximumMessageLength, section.Twitter.Recipients.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }

            IList<User> defaultUsers = new List<User>();

            foreach (DefaultUserConfigurationElement configuration in section.DefaultUsers)
            {
                defaultUsers.Add(new User { Name = configuration.Name, Email = configuration.Email, Role = configuration.Role });
            }

            Settings settings = new Settings(section.RedirectPermanently, section.UrlPerPage, section.BaseType, api, thumbnail, google, twitter, defaultUsers);

            return settings;
        }
    }
}