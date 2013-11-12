using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ServiceStack.Text.TupleSerializer
{
    internal static class TypeExtensions
    {
        private static readonly Type _tupleInterface = Type.GetType("System.ITuple");

        private static readonly Type[] _genericTupleTypes =
        {
            typeof (Tuple<>),
            typeof (Tuple<,>),
            typeof (Tuple<,,>),
            typeof (Tuple<,,,>),
            typeof (Tuple<,,,,>),
            typeof (Tuple<,,,,,>),
            typeof (Tuple<,,,,,,>),
            typeof (Tuple<,,,,,,,>),
        };

        public static bool IsTuple(this Type type)
        {
            return type.GetInterfaces().Contains(_tupleInterface);
        }

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

        public static Type FindTupleDefinition(this Type type, Type parentType = null)
        {
            if (type == null || !type.IsTuple())
            {
                return null;
            }

            if (type.IsGenericType)
            {
                if (type.IsGenericTypeDefinition && _genericTupleTypes.Contains(type))
                {
                    return parentType;
                }

                return type.GetGenericTypeDefinition().FindTupleDefinition(type);
            }

            return type.BaseType.FindTupleDefinition(type);
        }

        public static IEnumerable<Type> EnumerateTypeTrees(this IEnumerable<Type> rootTypes)
        {
            if(rootTypes == null) yield break;

            var alreadyChecked = new HashSet<Type>();
            var stack = new ConcurrentStack<Type>(rootTypes);

            Type currentType;
            while (stack.TryPop(out currentType))
            {
                if (alreadyChecked.Contains(currentType)) continue;

                alreadyChecked.Add(currentType);
                yield return currentType;

                var extractedTypes = currentType.ExtractTypes();
                if (extractedTypes.Length > 0)
                {
                    stack.PushRange(extractedTypes);
                }
            }
        }

        public static Type[] ExtractTypes(this Type rootType)
        {
            var types = new List<Type>();

            types.AddRange(rootType.GetProperties().Select(pi => pi.PropertyType));

            var baseType = rootType.BaseType;
            if (baseType != null && baseType != typeof(Object))
            {
                types.Add(baseType);
            }

            types.AddRange(rootType.GetInterfaces());

            if (rootType.IsGenericType && !rootType.IsGenericTypeDefinition)
            {
                types.AddRange(rootType.GetGenericArguments());
            }

            return types.ToArray();
        }
    }
}