namespace Shrinkr.Infrastructure.EntityFramework
{
    using System.Diagnostics;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory databaseFactory;
        private Database database;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            Check.Argument.IsNotNull(databaseFactory, "databaseFactory");

            this.databaseFactory = databaseFactory;
        }

        protected Database Database
        {
            [DebuggerStepThrough]
            get
            {
                return database ?? (database = databaseFactory.Get());
            }
        }

        public void Commit()
        {
            Database.Commit();
        }
    }
}