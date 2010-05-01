namespace Shrinkr.Infrastructure.EntityFramework
{
    using System;

    public interface IDatabaseFactory : IDisposable
    {
        Database Get();
    }
}