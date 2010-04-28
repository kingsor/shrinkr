namespace Shrinkr.Infrastructure.EntityFramework.IntegrationTests
{
    using System;
    using System.Configuration;
    using System.Data.Common;

    using EntityFramework;

    public abstract class DatabaseTest : IDisposable
    {
        protected readonly DatabaseFactory databasefactory;
        protected readonly QueryFactory queryFactory;
        protected readonly UnitOfWork unitOfWork;

        protected DatabaseTest()
        {
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["ShrinkrDatabase"];

            var providerName = connectionStringSettings.ProviderName;
            var connectionString = connectionStringSettings.ConnectionString;

            var providerFactory = DbProviderFactories.GetFactory(providerName);

            databasefactory = new DatabaseFactory(providerFactory, connectionString);
            queryFactory = new QueryFactory(true, true);

            unitOfWork = new UnitOfWork(databasefactory);
        }

        public virtual void Dispose()
        {
            databasefactory.Dispose();
        }
    }
}