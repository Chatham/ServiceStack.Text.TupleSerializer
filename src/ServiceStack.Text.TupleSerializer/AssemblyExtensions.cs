using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ServiceStack.Text.TupleSerializer
{
    internal static class AssemblyExtensions
    {
        internal static Func<string, bool> AlwaysTrueFilter
        {
            get { return s => true; }
        }

        public static HashSet<Type> GetPublicTuples(this ICollection<Assembly> assemblies, Func<string, bool> namespaceFilter = null)
        {
            if (assemblies.IsEmpty())
            {
                return new HashSet<Type>();
            }

            if (namespaceFilter == null)
            {
                namespaceFilter = AlwaysTrueFilter;
            }

            var tupleTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                if (assembly == null)
                {
                    continue;
                }

                var publicAssemblyTuples = 
                    assembly.GetTypes()
                    .Where(type => namespaceFilter(type.Namespace ?? string.Empty))
                    .EnumerateTypeTrees()
                    .GetTuples();
                
                tupleTypes.AddRange(publicAssemblyTuples);
            }
            return new HashSet<Type>(tupleTypes);
        }
    }
}