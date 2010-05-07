namespace Shrinkr.Repositories
{
    using System.Collections.Generic;

    using DomainObjects;

    public interface IRepository<TEntity> where TEntity : IEntity
    {
        void Add(TEntity entity);

        void Delete(TEntity entity);

        TEntity GetById(long id);

        IEnumerable<TEntity> All();
    }
}