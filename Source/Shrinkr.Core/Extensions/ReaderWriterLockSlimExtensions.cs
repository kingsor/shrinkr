namespace Shrinkr.Extensions
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    public static class ReaderWriterLockSlimExtensions
    {
        [DebuggerStepThrough]
        public static IDisposable ReadAndMaybeWrite(this ReaderWriterLockSlim instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            instance.EnterUpgradeableReadLock();

            return new SynchronizedCodeBlock(instance.ExitUpgradeableReadLock);
        }

        [DebuggerStepThrough]
        public static IDisposable Read(this ReaderWriterLockSlim instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            instance.EnterReadLock();

            return new SynchronizedCodeBlock(instance.ExitReadLock);
        }

        [DebuggerStepThrough]
        public static IDisposable Write(this ReaderWriterLockSlim instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            instance.EnterWriteLock();

            return new SynchronizedCodeBlock(instance.ExitWriteLock);
        }

        private sealed class SynchronizedCodeBlock : IDisposable
        {
            private readonly Action action;

            [DebuggerStepThrough]
            public SynchronizedCodeBlock(Action action)
            {
                this.action = action;
            }

            [DebuggerStepThrough]
            public void Dispose()
            {
                action();
            }
        }
    }
}