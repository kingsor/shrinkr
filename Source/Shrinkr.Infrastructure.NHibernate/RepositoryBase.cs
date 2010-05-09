namespace Shrinkr.Infrastructure.NHibernate
{
    using System.Collections.Generic;
    using System.Diagnostics;
    
    using DomainObjects;
   
    public abstract class RepositoryBase<TEntity> where TEntity : class, IEntity
    {
        private Database database;
        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            Check.Argument.IsNotNull(databaseFactory, "databaseFactory");
            
            DatabaseFactory = databaseFactory;
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }
        protected Database Database
        {
            [DebuggerStepThrough]
            get
            {
                return database ?? (database = DatabaseFactory.Get());
            }
        }

        public virtual void Add(TEntity entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Database.Save(entity);
        }
            
        public virtual void Delete(TEntity entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Database.Delete(entity);
        }

        public virtual TEntity GetById(long id)
        {
            return Database.GetById<TEntity>(id);
        }

        public virtual IEnumerable<TEntity> All()
        {
            return Database.CreateQuery<TEntity>();
        }
    }
}
