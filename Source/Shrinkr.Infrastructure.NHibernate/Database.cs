namespace Shrinkr.Infrastructure.NHibernate
{
    using System.Linq;
    using System.Diagnostics;

    using DomainObjects;

    using global::NHibernate.Linq;
    using ISession = global::NHibernate.ISession;
    using ITransaction = global::NHibernate.ITransaction;

    public class Database : Disposable
    {
        private readonly ISession session;
        private ITransaction transaction;

        private IQueryable<User> users;
        private IQueryable<ShortUrl> shortUrls;
        private IQueryable<Alias> aliases;
        private IQueryable<Visit> visits;
        private IQueryable<BannedDomain> bannedDomains;
        private IQueryable<BannedIPAddress> bannedIPAddresses;
        private IQueryable<ReservedAlias> restrictedAliases;
        private IQueryable<BadWord> badWords;

        public Database(ISession session)
        {
            Check.Argument.IsNotNull(session, "session");
            
            this.session = session;
        }

        public IQueryable<User> Users
        {
            [DebuggerStepThrough]
            get
            {
                return users ?? (users = CreateQuery<User>());
            }
        }

        public IQueryable<ShortUrl> ShortUrls
        {
            [DebuggerStepThrough]
            get
            {
                return shortUrls ?? (shortUrls = CreateQuery<ShortUrl>());
            }
        }

        public IQueryable<Alias> Aliases
        {
            [DebuggerStepThrough]
            get
            {
                return aliases ?? (aliases = CreateQuery<Alias>());
            }
        }

        public IQueryable<Visit> Visits
        {
            [DebuggerStepThrough]
            get
            {
                return visits ?? (visits = CreateQuery<Visit>());
            }
        }

        public IQueryable<BannedDomain> BannedDomains
        {
            [DebuggerStepThrough]
            get
            {
                return bannedDomains ?? (bannedDomains = CreateQuery<BannedDomain>());
            }
        }

        public IQueryable<BannedIPAddress> BannedIPAddresses
        {
            [DebuggerStepThrough]
            get
            {
                return bannedIPAddresses ?? (bannedIPAddresses = CreateQuery<BannedIPAddress>());
            }
        }

        public IQueryable<ReservedAlias> ReservedAliases
        {
            [DebuggerStepThrough]
            get
            {
                return restrictedAliases ?? (restrictedAliases = CreateQuery<ReservedAlias>());
            }
        }

        public IQueryable<BadWord> BadWords
        {
            [DebuggerStepThrough]
            get
            {
                return badWords ?? (badWords = CreateQuery<BadWord>());
            }
        }

        public virtual IQueryable<TEntity> CreateQuery<TEntity>() where TEntity : class, IEntity
        {
            return session.Linq<TEntity>();
        }

        public virtual TEntity GetById<TEntity>(long id) where TEntity : class, IEntity
        {
            Check.Argument.IsNotNegative(id, "id");
            return session.Get<TEntity>(id);
        }

        public virtual void Save<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            Check.Argument.IsNotNull(entity, "entity");
            EnsureTransaction();
            session.SaveOrUpdate(entity);
        }

        public virtual void Delete<TEntity>(TEntity entity) where TEntity: class, IEntity
        {
            Check.Argument.IsNotNull(entity, "entity");
            EnsureTransaction();
            session.Delete(entity);
        }

        public virtual void Commit()
        {
            EnsureTransaction();
            transaction.Commit();
        }

        private void EnsureTransaction()
        {
            //start new transaction if
            // 1) No transaction is initiated yet
            // 2) Existing transaction isn't active
            // 3) Existing transaction was committed
            // 4) Existing transaction was rolledback
            if(transaction == null || !transaction.IsActive || transaction.WasCommitted || transaction.WasRolledBack)
            {
                transaction = session.BeginTransaction();
            }
        }

        protected override void DisposeCore()
        {
            if (transaction != null)
            {
                if(transaction.IsActive)
                {
                    transaction.Rollback();
                }

                transaction.Dispose();
                transaction = null;
            }
            if (session != null)
            {
                if (session.IsOpen)
                {
                    session.Close();
                }

                session.Dispose();
            }
        }
    }
}