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
        public static T Reduce<T>(IEnumerable<T> source)
        {
            return default(T);
        }
    }
}
