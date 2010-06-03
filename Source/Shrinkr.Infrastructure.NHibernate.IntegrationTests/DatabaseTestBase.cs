namespace Shrinkr.Infrastructure.NHibernate.IntegrationTests
{
    using System;
    using System.Configuration;

    public abstract class DatabaseTestBase : IDisposable
    {
        protected DatabaseTestBase()
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings["shrinkr"];

            var providerName = connectionStringSettings.ProviderName;
            var connectionString = connectionStringSettings.ConnectionString;

            DatabaseFactory = new DatabaseFactory(providerName, connectionString);
            Database = DatabaseFactory.Get();
            UnitOfWork = new UnitOfWork(DatabaseFactory);
        }

        protected DatabaseFactory DatabaseFactory { get; private set; }

        protected UnitOfWork UnitOfWork { get; private set; }

        protected Database Database { get; private set; }

        public virtual void Dispose()
        {
            Database.Dispose();
            DatabaseFactory.Dispose();
        }
    }
}