namespace Shrinkr.DomainObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;

    using Infrastructure;

    public class User : IEntity
    {
        private ICollection<Alias> aliases;

        private DateTime createdAt;
        private DateTime lastActivityAt;

        private ApiSetting apiSetting;

        public User()
        {
            createdAt = SystemTime.Now();
            lastActivityAt = SystemTime.Now();
            aliases = new List<Alias>();
        }

        public virtual long Id
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Email
        {
            get;
            set;
        }

        public virtual bool IsLockedOut
        {
            get;
            set;
        }

        public virtual DateTime CreatedAt
        {
            [DebuggerStepThrough]
            get
            {
                return createdAt;
            }

            [DebuggerStepThrough]
            set
            {
                createdAt = value;
            }
        }

        public virtual DateTime LastActivityAt
        {
            [DebuggerStepThrough]
            get
            {
                return lastActivityAt;
            }

            [DebuggerStepThrough]
            set
            {
                lastActivityAt = value;
            }
        }

        public virtual Role Role
        {
            [DebuggerStepThrough]
            get
            {
                return (Role)InternalRole;
            }

            [DebuggerStepThrough]
            set
            {
                InternalRole = (int)value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual int InternalRole
        {
            [EditorBrowsable(EditorBrowsableState.Never)]
            get;
            [EditorBrowsable(EditorBrowsableState.Never)]
            set;
        }

        public virtual ICollection<Alias> Aliases
        {
            [DebuggerStepThrough]
            get { return aliases; }
        }

        public virtual ApiSetting ApiSetting
        {
            [DebuggerStepThrough]
            get
            {
                return apiSetting ?? (apiSetting = new ApiSetting());
            }

            [DebuggerStepThrough]
            set
            {
                Check.Argument.IsNotNull(value, "value");

                apiSetting = value;
            }
        }

        public virtual bool CanAccessApi
        {
            get
            {
                bool canAccess = (ApiSetting != null) &&
                                 ApiSetting.Allowed.GetValueOrDefault() &&
                                 (ApiSetting.DailyLimit == ApiSetting.InfiniteLimit || ApiSetting.DailyLimit > 0);

                return canAccess;
            }
        }

        public virtual void GenerateApiKey()
        {
            if (!CanAccessApi)
            {
                throw new InvalidOperationException(TextMessages.CannotGenerateApiKeyWhenApiAccessIsNotAllowed);
            }

            ApiSetting.Key = CreateApiKey();
        }

        public virtual void AllowApiAccess(int dailyLimit)
        {
            if (dailyLimit != ApiSetting.InfiniteLimit)
            {
                Check.Argument.IsNotNegative(dailyLimit, "dailyLimit");
            }

            ApiSetting.Allowed = true;
            ApiSetting.DailyLimit = dailyLimit;

            if (string.IsNullOrWhiteSpace(ApiSetting.Key))
            {
                GenerateApiKey();
            }
        }

        public virtual void BlockApiAccess()
        {
            ApiSetting.Allowed = false;
        }

        public virtual bool HasExceededDailyLimit()
        {
            DateTime lastOneDay = SystemTime.Now().AddDays(-1);

            bool exceeded = CanAccessApi &&
                            ((ApiSetting.DailyLimit != ApiSetting.InfiniteLimit) &&
                             (ApiSetting.DailyLimit <= Aliases.Count(alias => alias.CreatedAt > lastOneDay && alias.CreatedByApi)));

            return exceeded;
        }

        private static string CreateApiKey()
        {
            return Guid.NewGuid().ToString().ToLower(Culture.Invariant);
        }
    }
}