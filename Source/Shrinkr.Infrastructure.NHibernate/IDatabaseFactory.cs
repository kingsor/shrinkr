namespace Shrinkr.Infrastructure.Nhibernate
{
    using System;

    public interface IDatabaseFactory : IDisposable
    {
        Database Get();
    }
}
