namespace Shrinkr.Infrastructure.NHibernate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;
    using System.Reflection;

    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;

    using ISessionFactory = global::NHibernate.ISessionFactory;

    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private static readonly IDictionary<string, IPersistenceConfigurer> persistenceConfigurerMap = BuildPersistenceConfigurerMap();
        private static readonly object sessionFactorySyncLock = new object();

        private readonly string providerName;
        private readonly string connectionString;

        private static ISessionFactory sessionFactory;

        private Database database;

        public DatabaseFactory(string providerName, string connectionString)
        {
            Check.Argument.IsNotNullOrEmpty(providerName, "providerName");
            Check.Argument.IsNotNullOrEmpty(connectionString, "connectionString");

            this.providerName = providerName;
            this.connectionString = connectionString;
        }

        public Database Get()
        {
            if (database == null)
            {
                EnsureSessionFactory();

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

        private static Dictionary<string, IPersistenceConfigurer> BuildPersistenceConfigurerMap()
        {
            return new Dictionary<string, IPersistenceConfigurer>(StringComparer.OrdinalIgnoreCase)
                       {
                           { "System.Data.SqlClient", MsSqlConfiguration.MsSql2008 },
                           { "MySql.Data.MySqlClient", MySQLConfiguration.Standard }
                       };
        }

        private void EnsureSessionFactory()
        {
            if (sessionFactory == null)
            {
                lock (sessionFactorySyncLock)
                {
                    if (sessionFactory == null)
                    {
                        IPersistenceConfigurer configurer = persistenceConfigurerMap[providerName];

                        MethodInfo setConnectionString = configurer.GetType()
                                                                   .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod)
                                                                   .First(metheod => metheod.Name == "ConnectionString" && metheod.GetParameters().Any() && metheod.GetParameters()[0].ParameterType.Equals(typeof(string)));

                        setConnectionString.Invoke(configurer, new[] { connectionString });

                        sessionFactory = Fluently.Configure()
                                                 .Database(configurer)
                                                 .Mappings(mapping => mapping.FluentMappings.AddFromAssemblyOf<Database>())
                                                 .BuildSessionFactory();
                    }
                }
            }
        }
    }
}