namespace Shrinkr.DomainObjects
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;

    public class ShortUrl : IEntity
    {
        private readonly ICollection<Alias> aliases;

        public ShortUrl()
        {
            aliases = new List<Alias>();
        }

        public virtual long Id
        {
            get;
            set;
        }

        public virtual string Url
        {
            get;
            set;
        }

        public virtual string Domain
        {
            get;
            set;
        }

        public virtual string Hash
        {
            get;
            set;
        }

        public virtual string Title
        {
            get;
            set;
        }

        public virtual SpamStatus SpamStatus
        {
            [DebuggerStepThrough]
            get
            {
                return (SpamStatus)InternalSpamStatus;
            }

            [DebuggerStepThrough]
            set
            {
                InternalSpamStatus = (int)value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual int InternalSpamStatus
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
    }
}