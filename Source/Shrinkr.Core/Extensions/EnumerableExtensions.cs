namespace Shrinkr.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public static class EnumerableExtensions
    {
        [DebuggerStepThrough]
        public static void Each<T>(this IEnumerable<T> instance, Action<T> action)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotNull(action, "action");

            foreach (T item in instance)
            {
                action(item);
            }
        }
    }
}