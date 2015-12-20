using System;
using System.Collections.Generic;

namespace Cafemoca.MinecraftCommandStudio.Internals.Extensions
{
    public static class IEnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> source, T target)
        {
            var index = 0;
            foreach (T item in source)
            {
                if (EqualityComparer<T>.Default.Equals(item, target))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var index = 0;
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public static string JoinString(this IEnumerable<string> source, string separator)
        {
            if (source == null)
            {
                return string.Empty;
            }
            if (separator == null)
            {
                return source.ToString();
            }
            return string.Join(separator, source);
        }
    }
}
