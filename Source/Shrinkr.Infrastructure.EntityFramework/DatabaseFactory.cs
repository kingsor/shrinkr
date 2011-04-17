namespace Shrinkr.Infrastructure.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Linq;

    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Infrastructure;
    
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private static readonly object syncObject = new object();

        private readonly DbProviderFactory providerFactory;
        private readonly string connectionString;

        private static DbCompiledModel model;

        private Database database;

        public DatabaseFactory(DbProviderFactory providerFactory, string connectionString)
        {
            Check.Argument.IsNotNull(providerFactory, "providerFactory");
            Check.Argument.IsNotNullOrEmpty(connectionString, "connectionString");

            this.providerFactory = providerFactory;
            this.connectionString = connectionString;
        }

        public virtual Database Get()
        {
            if (database == null)
            {
                DbConnection connection = providerFactory.CreateConnection();

                if (connection != null)
                {
                    connection.ConnectionString = connectionString;

                    if (model == null)
                    {
                        lock (syncObject)
                        {
                            if (model == null)
                            {
                                model = CreateDbModel(connection);
                            }
                        }
                    }

                    database = model.CreateObjectContext<Database>(connection);
                }

                return database;
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

        private static DbCompiledModel CreateDbModel(DbConnection connection)
        {
            var modelBuilder = new DbModelBuilder();

            IEnumerable<Type> configurationTypes = typeof(DatabaseFactory).Assembly
                .GetTypes()
                .Where(
                    type =>
                    type.IsPublic && type.IsClass && !type.IsAbstract && !type.IsGenericType && type.BaseType != null &&
                    type.BaseType.IsGenericType &&
                    (type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>) ||
                     type.BaseType.GetGenericTypeDefinition() == typeof(ComplexTypeConfiguration<>)) && (type.GetConstructor(Type.EmptyTypes) != null));

            foreach (var configuration in configurationTypes.Select(Activator.CreateInstance))
            {
                modelBuilder.Configurations.Add((dynamic)configuration);
            }
            
            return modelBuilder.Build(connection).Compile();
        }
    }
}