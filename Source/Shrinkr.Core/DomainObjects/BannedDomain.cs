namespace Shrinkr.DomainObjects
{
    public class BannedDomain : IEntity
    {
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
    }
}