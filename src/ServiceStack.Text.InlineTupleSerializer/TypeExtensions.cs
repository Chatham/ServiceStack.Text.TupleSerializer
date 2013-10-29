using System;
using System.Linq;

namespace ServiceStack.Text.InlineTupleSerializer
{
    internal static class TypeExtensions
    {
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
            return type.GetInterfaces().Contains(Type.GetType("System.ITuple"));
        }

        public static Type FindTupleDefinition(this Type type, Type parentType = null)
        {
            if (!type.IsTuple())
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

            var derived = type.BaseType;
            if (derived != null && derived != typeof(Object))
            {
                return derived.FindTupleDefinition(type);
            }

            return null;
        }
    }
}