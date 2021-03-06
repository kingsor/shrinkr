namespace Shrinkr.Web.CommunityStack
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web;

    using Autofac;

    using MvcExtensions;

    using DomainObjects;
    using Infrastructure;
    using Infrastructure.NHibernate;
    using Services;

    public class RegisterServices : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Http>().As<IHttp>().SingleInstance();
            builder.RegisterType<PageGlimpseThumbnail>().As<IThumbnail>().SingleInstance();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.Register(c => new CacheManager(HttpRuntime.Cache)).As<ICacheManager>().SingleInstance();
            builder.RegisterType<ExternalContentService>().As<IExternalContentService>().SingleInstance();
            builder.RegisterType<GoogleSafeBrowsing>().As<IGoogleSafeBrowsing>().SingleInstance();
            builder.RegisterType<UrlResolver>().As<IUrlResolver>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<ShortUrlService>().As<IShortUrlService>().InstancePerLifetimeScope();
            builder.RegisterType<AdministrativeService>().As<IAdministrativeService>().InstancePerLifetimeScope();
            builder.RegisterType<OpenIdRelyingParty>().As<IOpenIdRelyingParty>().InstancePerDependency();
            builder.RegisterType<FormsAuthentication>().As<IFormsAuthentication>().SingleInstance();
            builder.RegisterType<Cookie>().As<ICookie>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(RepositoryBase<>).Assembly)
                   .Where(type => type.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(ISpamDetector).Assembly)
                   .Where(type => type.IsClass && !type.IsAbstract && typeof(ISpamDetector).IsAssignableFrom(type))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IBackgroundService).Assembly)
                   .Where(type => type.IsClass && !type.IsAbstract && typeof(IBackgroundService).IsAssignableFrom(type))
                   .AsImplementedInterfaces()
                   .SingleInstance();

            Settings settings = CreateSettings();
            builder.RegisterInstance(settings).As<Settings>().SingleInstance();

            builder.Register(c => new BaseX(c.Resolve<Settings>().BaseType)).As<IBaseX>().SingleInstance();

            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["Shrinkr"];

            string providerName = connectionStringSettings.ProviderName;
            string connectionString = connectionStringSettings.ConnectionString;

            builder.Register(c => new DatabaseFactory(providerName, connectionString)).As<IDatabaseFactory>().InstancePerLifetimeScope();
            builder.Register(c => new QueryFactory(settings.BaseType == BaseType.BaseSixtyTwo)).As<IQueryFactory>().SingleInstance();

            builder.RegisterType<CompressAttribute>().InstancePerDependency();
        }

        private static Settings CreateSettings()
        {
            var section = (SettingConfigurationSection)ConfigurationManager.GetSection(SettingConfigurationSection.SectionName);

            var api = new ApiSettings(section.Api.Allowed, section.Api.DailyLimit);

            var thumbnail = new ThumbnailSettings(section.Thumbnail.ApiKey, section.Thumbnail.Endpoint);

            var google = new GoogleSafeBrowsingSettings(section.Google.ApiKey, section.Google.Endpoint, HttpContext.Current.Server.MapPath(section.Google.PhishingFile), HttpContext.Current.Server.MapPath(section.Google.MalwareFile));

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

            var settings = new Settings(section.RedirectPermanently, section.UrlPerPage, section.BaseType, api, thumbnail, google, twitter, defaultUsers);

            return settings;
        }
    }
}