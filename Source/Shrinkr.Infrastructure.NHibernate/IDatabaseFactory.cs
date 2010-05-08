namespace Shrinkr.Infrastructure.NHibernate
{
    using System;

    [CLSCompliant(false)]
    public interface IDatabaseFactory : IDisposable
    {
        Database Get();
    }
}