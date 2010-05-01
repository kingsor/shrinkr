namespace Shrinkr.Infrastructure.EntityFramework.IntegrationTests
{
    using System;
    using System.Configuration;
    using System.Data.Common;

    using EntityFramework;

    public abstract class DatabaseTest : IDisposable
    {
        protected readonly DatabaseFactory Databasefactory;
        protected readonly QueryFactory QueryFactory;
        protected readonly UnitOfWork UnitOfWork;

        protected DatabaseTest()
        {
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["ShrinkrDatabase"];

            var providerName = connectionStringSettings.ProviderName;
            var connectionString = connectionStringSettings.ConnectionString;

            var providerFactory = DbProviderFactories.GetFactory(providerName);

            Databasefactory = new DatabaseFactory(providerFactory, connectionString);
            QueryFactory = new QueryFactory(true, true);

            UnitOfWork = new UnitOfWork(Databasefactory);
        }

        public virtual void Dispose()
        {
            Databasefactory.Dispose();
        }
    }
}