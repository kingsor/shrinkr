namespace Shrinkr.Infrastructure.NHibernate.Query
{
    /// Reserved for future use
    public abstract class QueryBase<TEntity> : IQuery<TEntity>
    {
        public abstract TEntity Execute(Database database);
    }
}