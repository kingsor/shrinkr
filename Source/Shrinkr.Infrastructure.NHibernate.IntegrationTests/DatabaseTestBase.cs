namespace Shrinkr.Infrastructure.NHibernate.IntegrationTests
{
    using System;
    using System.Configuration;

    public abstract class DatabaseTestBase : IDisposable
    {
        private readonly DatabaseFactory factory;

        protected DatabaseTestBase()
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings["shrinkr"];

            var providerName = connectionStringSettings.ProviderName;
            var connectionString = connectionStringSettings.ConnectionString;

            factory = new DatabaseFactory(providerName, connectionString);

            Database = factory.Get();
        }

        protected Database Database { get; private set; }

        public virtual void Dispose()
        {
            factory.Dispose();
        }
    }
}