namespace Shrinkr.Infrastructure.Nhibernate
{
    using System.Diagnostics;

    using NHibernate;
    using NHibernate.Linq;

    using DomainObjects;

    public class Database : Disposable
    {
        private readonly ISession session;

        private INHibernateQueryable<User> users;
        private INHibernateQueryable<ShortUrl> shortUrls;
        private INHibernateQueryable<Alias> aliases;
        private INHibernateQueryable<Visit> visits;

        private INHibernateQueryable<BannedDomain> bannedDomains;
        private INHibernateQueryable<BannedIPAddress> bannedIPAddresses;
        private INHibernateQueryable<ReservedAlias> restrictedAliases;
        private INHibernateQueryable<BadWord> badWords;

        public Database(ISession session)
        {
            this.session = session;    
        }

        public INHibernateQueryable<User> Users
        {
            [DebuggerStepThrough]
            get
            {
                return users ?? (users = NHibernateQueryable<User>());
            }
        }

        public INHibernateQueryable<ShortUrl> ShortUrls
        {
            [DebuggerStepThrough]
            get
            {
                return shortUrls ?? (shortUrls = NHibernateQueryable<ShortUrl>());
            }
        }

        public INHibernateQueryable<Alias> Aliases
        {
            [DebuggerStepThrough]
            get
            {
                return aliases ?? (aliases = NHibernateQueryable<Alias>());
            }
        }

        public INHibernateQueryable<Visit> Visits
        {
            [DebuggerStepThrough]
            get
            {
                return visits ?? (visits = NHibernateQueryable<Visit>());
            }
        }

        public INHibernateQueryable<BannedDomain> BannedDomains
        {
            [DebuggerStepThrough]
            get
            {
                return bannedDomains ?? (bannedDomains = NHibernateQueryable<BannedDomain>());
            }
        }

        public INHibernateQueryable<BannedIPAddress> BannedIPAddresses
        {
            [DebuggerStepThrough]
            get
            {
                return bannedIPAddresses ?? (bannedIPAddresses = NHibernateQueryable<BannedIPAddress>());
            }
        }

        public INHibernateQueryable<ReservedAlias> ReservedAliases
        {
            [DebuggerStepThrough]
            get
            {
                return restrictedAliases ?? (restrictedAliases = NHibernateQueryable<ReservedAlias>());
            }
        }

        public INHibernateQueryable<BadWord> BadWords
        {
            [DebuggerStepThrough]
            get
            {
                return badWords ?? (badWords = NHibernateQueryable<BadWord>());
            }
        }

        public virtual INHibernateQueryable<TEntity> NHibernateQueryable<TEntity>() where TEntity : class, IEntity
        {
            return session.Linq<TEntity>();
        }

        public virtual void Save<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            Check.Argument.IsNotNull(entity, "entity");

            session.SaveOrUpdate(entity);
        }

        protected override void DisposeCore()
        {
            if(session != null)
            {
                if(session.IsOpen)
                {
                    session.Close();
                }
                session.Dispose();
            }
        }
    }
}
