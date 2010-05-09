namespace Shrinkr.Infrastructure.NHibernate.IntegrationTests
{
    using System;
    using System.Configuration;

    public abstract class DatabaseTestBase : IDisposable
    {
        private readonly DatabaseFactory factory;
        private readonly UnitOfWork unitOfWork;
        private readonly Database database;
        protected DatabaseTestBase()
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings["shrinkr"];

            var providerName = connectionStringSettings.ProviderName;
            var connectionString = connectionStringSettings.ConnectionString;

            factory = new DatabaseFactory(providerName, connectionString);
            database = factory.Get();
            unitOfWork = new UnitOfWork(factory);
            
        }

        protected DatabaseFactory DatabaseFactory
        {
            get { return factory; }
        }
        protected UnitOfWork UnitOfWork
        {
            get { return unitOfWork; }
        }
        protected Database Database
        {
            get { return database; }
        }

        public virtual void Dispose()
        {
            database.Dispose();
            factory.Dispose();
        }
    }
}