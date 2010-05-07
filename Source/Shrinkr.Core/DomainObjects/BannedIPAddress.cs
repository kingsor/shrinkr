namespace Shrinkr.DomainObjects
{
    public class BannedIPAddress : IEntity
    {
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
    }
}