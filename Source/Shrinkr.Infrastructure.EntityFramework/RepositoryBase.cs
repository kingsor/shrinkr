namespace Shrinkr.Infrastructure.EntityFramework
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using DomainObjects;

    public abstract class RepositoryBase<TEntity> where TEntity : class, IEntity
    {
        private Database database;

        protected RepositoryBase(IDatabaseFactory databaseFactory, IQueryFactory queryFactory)
        {
            Check.Argument.IsNotNull(databaseFactory, "databaseFactory");
            Check.Argument.IsNotNull(queryFactory, "queryFactory");

            DatabaseFactory = databaseFactory;
            QueryFactory = queryFactory;
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected IQueryFactory QueryFactory
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

            Database.ObjectSet<TEntity>().AddObject(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Database.ObjectSet<TEntity>().DeleteObject(entity);
        }

        public virtual TEntity GetById(long id)
        {
            return Database.ObjectSet<TEntity>().SingleOrDefault(entity => entity.Id == id);
        }

        public virtual IEnumerable<TEntity> All()
        {
            return Database.ObjectSet<TEntity>();
        }
    }
}