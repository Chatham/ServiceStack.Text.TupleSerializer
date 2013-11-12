using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceStack.Text.TupleSerializer
{
    internal static class CollectionExtensions
    {
        public static HashSet<Type> GetTuples(this IEnumerable<Type> types)
        {
            if (types == null)
            {
                return new HashSet<Type>();
            }

            var enumTypes =
                from type in types.AsParallel()
                where type.IsTuple()
                select type;

            return new HashSet<Type>(enumTypes);
        }
    }
}