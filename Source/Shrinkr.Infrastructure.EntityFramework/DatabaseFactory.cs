namespace Shrinkr.Infrastructure.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Linq;

    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Infrastructure;

    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private static readonly DbModel model = CreateDbModel();

        private readonly DbProviderFactory providerFactory;
        private readonly string connectionString;

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
                connection.ConnectionString = connectionString;

                database = model.CreateObjectContext<Database>(connection);
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

        private static DbModel CreateDbModel()
        {
            var modelBuilder = new ModelBuilder();

            IEnumerable<Type> configurationTypes = typeof(DatabaseFactory).Assembly
                                                                          .GetTypes()
                                                                          .Where(type => type.IsPublic && type.IsClass && !type.IsAbstract && !type.IsGenericType && typeof(StructuralTypeConfiguration).IsAssignableFrom(type) && (type.GetConstructor(Type.EmptyTypes) != null));

            foreach (StructuralTypeConfiguration configuration in configurationTypes.Select(type => (StructuralTypeConfiguration)Activator.CreateInstance(type)))
            {
                modelBuilder.Configurations.Add(configuration);
            }

            return modelBuilder.CreateModel();
        }
    }
}