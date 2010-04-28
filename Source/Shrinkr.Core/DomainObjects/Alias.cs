namespace Shrinkr.DomainObjects
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Infrastructure;

    public class Alias : IEntity
    {
        private readonly ICollection<Visit> visits;
        private DateTime createdAt;

        public Alias()
        {
            visits = new List<Visit>();
            createdAt = SystemTime.Now();
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

        public virtual string IPAddress
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

        public virtual bool CreatedByApi
        {
            get;
            set;
        }

        public virtual ICollection<Visit> Visits
        {
            [DebuggerStepThrough]
            get { return visits; }
        }

        public virtual User User
        {
            get;
            set;
        }

        public virtual ShortUrl ShortUrl
        {
            get;
            set;
        }
    }
}