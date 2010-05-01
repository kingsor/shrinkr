namespace Shrinkr.Infrastructure.EntityFramework
{
    using System.Data.EntityClient;
    using System.Data.Objects;
    using System.Diagnostics;

    using DomainObjects;

    public class Database : ObjectContext
    {
        private IObjectSet<User> users;
        private IObjectSet<ShortUrl> shortUrls;
        private IObjectSet<Alias> aliases;
        private IObjectSet<Visit> visits;

        private IObjectSet<BannedDomain> bannedDomains;
        private IObjectSet<BannedIPAddress> bannedIPAddresses;
        private IObjectSet<ReservedAlias> restrictedAliases;
        private IObjectSet<BadWord> badWords;

        public Database(EntityConnection connection) : base(connection)
        {
            ContextOptions.LazyLoadingEnabled = true;
            ContextOptions.ProxyCreationEnabled = true;
        }

        public IObjectSet<User> Users
        {
            [DebuggerStepThrough]
            get
            {
                return users ?? (users = ObjectSet<User>());
            }
        }

        public IObjectSet<ShortUrl> ShortUrls
        {
            [DebuggerStepThrough]
            get
            {
                return shortUrls ?? (shortUrls = ObjectSet<ShortUrl>());
            }
        }

        public IObjectSet<Alias> Aliases
        {
            [DebuggerStepThrough]
            get
            {
                return aliases ?? (aliases = ObjectSet<Alias>());
            }
        }

        public IObjectSet<Visit> Visits
        {
            [DebuggerStepThrough]
            get
            {
                return visits ?? (visits = ObjectSet<Visit>());
            }
        }

        public IObjectSet<BannedDomain> BannedDomains
        {
            [DebuggerStepThrough]
            get
            {
                return bannedDomains ?? (bannedDomains = ObjectSet<BannedDomain>());
            }
        }

        public IObjectSet<BannedIPAddress> BannedIPAddresses
        {
            [DebuggerStepThrough]
            get
            {
                return bannedIPAddresses ?? (bannedIPAddresses = ObjectSet<BannedIPAddress>());
            }
        }

        public IObjectSet<ReservedAlias> ReservedAliases
        {
            [DebuggerStepThrough]
            get
            {
                return restrictedAliases ?? (restrictedAliases = ObjectSet<ReservedAlias>());
            }
        }

        public IObjectSet<BadWord> BadWords
        {
            [DebuggerStepThrough]
            get
            {
                return badWords ?? (badWords = ObjectSet<BadWord>());
            }
        }

        public virtual IObjectSet<TEntity> ObjectSet<TEntity>() where TEntity : class, IEntity
        {
            return CreateObjectSet<TEntity>();
        }

        public virtual void Commit()
        {
            SaveChanges();
        }
    }
}