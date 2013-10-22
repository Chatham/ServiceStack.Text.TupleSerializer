using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ServiceStack.Text.InlineTupleSerializer
{
    internal class TupleSerializationHelpers<TTuple> where TTuple
        : IStructuralEquatable, IStructuralComparable, IComparable
    {
        private const string _delimeter = "-";

        public int TupleMemberCount { get; private set; }

        public Type[] TupleSubtypes { get; private set; }

        public IList<MethodInfo> TupleMemberProxies { get; private set; }

        public TupleSerializationHelpers()
        {
            if (!typeof(TTuple).IsTuple())
            {
                throw new InvalidOperationException();
            }

            TupleMemberCount = typeof(TTuple).GetGenericArguments().Count();
            TupleSubtypes = typeof(TTuple).GetGenericArguments();
            TupleMemberProxies = Enumerable.Range(1, TupleMemberCount)
                .Select(i => typeof(TTuple).GetProperty(string.Concat("Item", i.ToString(CultureInfo.InvariantCulture))))
                .Select(pi => pi.GetGetMethod())
                .ToList();
        }

        public TTuple GetTupleFrom(string stringValue)
        {
            return DeserializeTuple(stringValue);//, Cache);
        }

        public string GetStringValue(TTuple tupleValue)
        {
            return SerializeTuple(tupleValue);//, Cache);
        }

        internal string SerializeTuple(TTuple tupleValue)
        {
            var stringBuilder = new StringBuilder();
            var delimeter = "";
            foreach (var tupleMemberProxy in TupleMemberProxies)
            {
                stringBuilder.Append(delimeter);
                stringBuilder.Append(tupleMemberProxy.Invoke(tupleValue, new object[] {}));
                delimeter = _delimeter;
            }

            return stringBuilder.ToString();
        }

        internal TTuple DeserializeTuple(string stringValue)
        {
            var stringValues = stringValue.Split(new []{_delimeter}, StringSplitOptions.None);

            if (stringValues.Length != TupleMemberCount)
            {
                throw new InvalidOperationException();
            }

            var objects = MapStringValuesToObjects(stringValues);

            return (TTuple) Activator.CreateInstance(typeof (TTuple), objects);
        }

        internal object[] MapStringValuesToObjects(string[] stringValues)
        {
            var objects = new object[TupleMemberCount];

            for (var i = 0; i < TupleMemberCount; i++)
            {
                var converter = TypeDescriptor.GetConverter(TupleSubtypes[i]);
                var result = converter.ConvertFrom(stringValues[i]);
                objects[i] = result;
            }

            return objects;
        }
    }
}