namespace Shrinkr.Infrastructure
{
    using System;

    public interface IDelegateReference
    {
        Delegate Target
        {
            get;
        }
    }
}