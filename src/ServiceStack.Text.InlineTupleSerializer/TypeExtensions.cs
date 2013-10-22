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
            return type.IsGenericTypeDefinition
                ? _genericTupleTypes.Contains(type)
                : _genericTupleTypes.Contains(type.GetGenericTypeDefinition());
        }
    }
}