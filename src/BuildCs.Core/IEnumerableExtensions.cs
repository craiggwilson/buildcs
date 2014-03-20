using System;
using System.Collections.Generic;

namespace BuildCs
{
    public static class IEnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach(var item in items)
                action(item);
        }
    }
}