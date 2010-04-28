namespace Shrinkr.Extensions
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public static class CollectionExtensions
    {
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(this ICollection<T> instance)
        {
            return (instance == null) || (instance.Count == 0);
        }

        [DebuggerStepThrough]
        public static bool IsEmpty<T>(this ICollection<T> instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            return instance.Count == 0;
        }

        [DebuggerStepThrough]
        public static void AddRange<T>(this ICollection<T> instance, IEnumerable<T> collection)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotNull(collection, "collection");

            foreach (T item in collection)
            {
                instance.Add(item);
            }
        }
    }
}