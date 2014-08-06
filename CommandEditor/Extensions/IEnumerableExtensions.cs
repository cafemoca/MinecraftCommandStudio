using System.Collections.Generic;

namespace Cafemoca.CommandEditor.Extensions
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
    }
}
