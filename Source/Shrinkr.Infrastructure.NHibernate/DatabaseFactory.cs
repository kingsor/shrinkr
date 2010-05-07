namespace Shrinkr.Infrastructure.Nhibernate
{
    using System;
    using System.Diagnostics;

    using NHibernate;
    using NHibernate.Cfg;

    [CLSCompliant(false)]
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private readonly Configuration configuration;

        public DatabaseFactory(DatabaseConfigurationBuilderBase dbConfiguration)
        {
            configuration = dbConfiguration.Configuration;
        }

        private Database database;
        
        public Database Get()
        {
            if (database == null)
            {
                ISessionFactory sessionFactory = configuration.BuildSessionFactory();
                
                database = new Database(sessionFactory.OpenSession());
            }
            return database;
        }

        [DebuggerStepThrough]
        protected override void DisposeCore()
        {
            if (database != null)
            {
                database.Dispose();
            }
        }
    }

}
