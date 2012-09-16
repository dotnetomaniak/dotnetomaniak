namespace Kigg
{
    using System.Collections.Generic;

    public static class CollectionExtension
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return (collection == null) || (collection.Count == 0);
        }
    }
}