using System;
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
            var stack = new Stack<Type>(rootTypes);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (alreadyChecked.Contains(current)) continue;

                alreadyChecked.Add(current);
                yield return current;

                foreach (var property in current.GetProperties())
                {
                    stack.Push(property.PropertyType);
                }

                var baseType = current.BaseType;
                if (baseType != null && baseType != typeof(Object))
                {
                    stack.Push(baseType);
                }

                foreach (var type in current.GetInterfaces())
                {
                    stack.Push(type);
                }

                if (current.IsGenericType && !current.IsGenericTypeDefinition)
                {
                    foreach (var type in current.GetGenericArguments())
                    {
                        stack.Push(type);
                    }
                }
            }
        }
    }
}