namespace Shrinkr.DomainObjects
{
    public class BadWord : IEntity
    {
        public virtual long Id
        {
            get;
            set;
        }

        public virtual string Expression
        {
            get;
            set;
        }
    }
}