using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    public static class IEnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach(var item in items)
                action(item);
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> @this)
        {
            return @this.Aggregate(Enumerable.Empty<T>(), (acc, c) => acc.Concat(c));
        }
    }
}