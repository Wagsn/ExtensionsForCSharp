using System;
using System.Collections.Generic;
using System.Text;

namespace WS.Extensions.Collections.Generic
{
    /// <summary>
    /// System.Collections.Generic Extension
    /// </summary>
    public static class CollectionsExtension
    {
        public static void Refresh<T>(this IList<T> list, IEnumerable<T> other)
        {
            foreach(var item in other)
            {
                list.Add(item);
            }
        }
    }
}
