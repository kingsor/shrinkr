namespace Shrinkr.Infrastructure.Nhibernate
{
    using System;

    using NHibernate.Cfg;

    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;

    /// <summary>
    /// Database configuration class for NHibernate.
    /// </summary>
    /// <remarks>
    /// When using this class with IoC, make sure to configure instances lifetime
    /// as singleton.
    /// </remarks>
    [CLSCompliant(false)]
    public class DatabaseConfigurationBuilderBase
    {
        private readonly Configuration configuration;
        public Configuration Configuration
        {
            get
            {
                return configuration;
            }
        }

        protected DatabaseConfigurationBuilderBase(IPersistenceConfigurer persistenceConfigurer)
        {
            configuration = CreateConfiguraiton(persistenceConfigurer);
        }


        private static Configuration CreateConfiguraiton(IPersistenceConfigurer persistenceConfigurer)
        {
            return Fluently.Configure()
               .Database(persistenceConfigurer)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Database>())
                .BuildConfiguration();
        }
    }

    /// <summary>
    /// SQL Server 2008 configuration class for NHibernate.
    /// </summary>
    [CLSCompliant(false)]
    public class SqlDatabaseConfigurationBuilder : DatabaseConfigurationBuilderBase
    {
        public SqlDatabaseConfigurationBuilder(string connectionString)
            : base(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
        {
        }
    }

    /// <summary>
    /// Standard MySQL configuration class for NHibernate.
    /// </summary>
    [CLSCompliant(false)]
    public class MySqlDatabaseConfigurationBuilder : DatabaseConfigurationBuilderBase
    {
        public MySqlDatabaseConfigurationBuilder(string connectionString)
            : base(MySQLConfiguration.Standard.ConnectionString(connectionString))
        {
        }
    }
}
