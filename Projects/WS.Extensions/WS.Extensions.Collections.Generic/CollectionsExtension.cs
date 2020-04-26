using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// System.Collections.Generic Extension
    /// </summary>
    public static class CollectionsExtension
    {
        /// <summary>
        /// Clear this list and AddRange other
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="other"></param>
        public static void Refresh<T>(this IList<T> list, IEnumerable<T> other)
        {
            list.Clear();
            foreach(var item in other)
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// [x1, x2, x3, x4].Reduce(f) = f(f(f(x1, x2), x3), x4).
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="source">Source IEnumerable.</param>
        /// <param name="merge">Merge expression.</param>
        /// <returns>return default if source is null or not exist any element.</returns>
        public static T Reduce<T>(this IEnumerable<T> source, Func<T,T,T> merge)
        {
            if(source== null || !source.Any()) return default(T);
            var result = source.First();
            foreach (var item in source.Skip(1))
            {
                result = merge(result, item);
            }
            return result;
        }

        /// <summary>
        /// Paging query
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="source">Source IEnumerable.</param>
        /// <param name="size">Size of page.</param>
        /// <param name="index">index of page.</param>
        /// <returns>source.Skip(index * size).Take(size);</returns>
        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int size, int index = 0)
        {
            return source.Skip(index * size).Take(size);
        }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T,T,bool> comparer)
        {
            throw new NotImplementedException();
        }
    }
}
