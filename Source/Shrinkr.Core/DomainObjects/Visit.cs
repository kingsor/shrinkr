namespace Shrinkr.DomainObjects
{
    using System;
    using System.Diagnostics;

    using Infrastructure;

    public class Visit : IEntity
    {
        private DateTime createdAt;

        public Visit()
        {
            createdAt = SystemTime.Now();
        }

        public virtual long Id
        {
            get;
            set;
        }

        public virtual string IPAddress
        {
            get;
            set;
        }

        public virtual string Browser
        {
            get;
            set;
        }

        public virtual Referrer Referrer
        {
            get;
            set;
        }

        public virtual long? GeoCode
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

        public virtual Alias Alias
        {
            get;
            set;
        }
    }
}