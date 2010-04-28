namespace Shrinkr.DomainObjects
{
    public class ReservedAlias : IEntity
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