namespace Shrinkr.Infrastructure.NHibernate
{
    using System;

    public interface IDatabaseFactory : IDisposable
    {
        Database Get();
    }
}