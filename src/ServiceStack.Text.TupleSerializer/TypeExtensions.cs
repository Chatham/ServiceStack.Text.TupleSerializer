using System;
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
    }
}