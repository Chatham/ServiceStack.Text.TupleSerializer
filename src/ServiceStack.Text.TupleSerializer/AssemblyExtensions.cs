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
                    from publicTupleType in assembly.GatherTypes(namespaceFilter).GetTuples()
                    select publicTupleType;

                tupleTypes.AddRange(publicAssemblyTuples);
            }
            return new HashSet<Type>(tupleTypes);
        }

        internal static List<Type> GatherTypes(this Assembly assembly, Func<string, bool> namespaceFilter)
        {
            var types = new List<Type>();

            foreach (var type in assembly.GetTypes())
            {
                if (!namespaceFilter(type.Namespace ?? string.Empty))
                {
                    continue;
                }

                types.Add(type);

                foreach (var property in type.GetProperties())
                {
                    types.Add(property.PropertyType);
                }
            }
            
            return types;
        }
    }
}