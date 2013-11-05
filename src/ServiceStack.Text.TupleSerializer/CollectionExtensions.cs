using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceStack.Text.InlineTupleSerializer
{
    internal static class CollectionExtensions
    {
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        public static HashSet<Type> GetTuples(this ICollection<Type> types)
        {
            if (types.IsEmpty())
            {
                return new HashSet<Type>();
            }

            var enumTypes =
                (from type in types.AsParallel()
                 where type.IsTuple()
                 select type
                ).ToList();

            return new HashSet<Type>(enumTypes);
        }
    }
}