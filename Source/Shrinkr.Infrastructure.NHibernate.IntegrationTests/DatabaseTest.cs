using Xunit;
using Xunit.Extensions;

namespace Shrinkr.Infrastructure.Nhibernate.IntegrationTests
{
    using System;
    using System.Configuration;
    using System.Data.Common;

    public class DatabaseTest : IDisposable
    {
        protected DatabaseFactory Databasefactory;

        protected DatabaseTest()
        {
            

        }

        public virtual void Dispose()
        {
            Databasefactory.Dispose();
        }

        //[Theory]
        //public void FakeTest()
        //{
        //    ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["Shrinkr"];

        //    var connectionString = connectionStringSettings.ConnectionString;

        //    Databasefactory = new DatabaseFactory(new SqlDatabaseConfigurationBuilder(connectionString));
        //    Database db = Databasefactory.Get();
        //    db.Dispose();
        //    Assert.True(0 == 0);
        //}
    }
}
            
