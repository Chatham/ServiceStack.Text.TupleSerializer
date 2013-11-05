﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using ServiceStack.Text.TupleSerializer.Api;

namespace ServiceStack.Text.TupleSerializer
{
    internal class TupleSerializationHelpers<TTuple> : ITupleSerializer<TTuple> 
        where TTuple : IStructuralEquatable, IStructuralComparable, IComparable
    {
        internal const string DELIMETER = "-";

        internal readonly TupleReflectionProxy<TTuple> _tupleInfo;
        internal readonly ICache<TTuple, string> _serializationCache;
        internal readonly ICache<string, TTuple> _deserializationCache;

        public TupleSerializationHelpers()
            : this(new TupleReflectionProxy<TTuple>(), TupleSerializationCache<TTuple>.SerializeCache, TupleSerializationCache<TTuple>.DeserializeCache)
        {
        }

        public TupleSerializationHelpers(
            ICache<TTuple, string> serializationCache,
            ICache<string, TTuple> deserializationCache)
            : this(new TupleReflectionProxy<TTuple>(), serializationCache, deserializationCache)
        {
        }

        public TupleSerializationHelpers(
            TupleReflectionProxy<TTuple> tupleInfo,
            ICache<TTuple, string> serializationCache,
            ICache<string, TTuple> deserializationCache)
        {
            _tupleInfo = tupleInfo;
            _serializationCache = serializationCache; 
            _deserializationCache = deserializationCache;
        }

        public TTuple GetTupleFrom(string stringValue)
        {
            return DeserializeTuple(stringValue, _deserializationCache);
        }

        public string GetStringValue(TTuple tupleValue)
        {
            return SerializeTuple(tupleValue, _serializationCache);
        }

        internal string SerializeTuple(TTuple tupleValue, ICache<TTuple, string> cache)
        {
            return cache.GetOrAdd(tupleValue, SerializeTuple);
        }

        internal string SerializeTuple(TTuple tupleValue)
        {
            var stringBuilder = new StringBuilder();
            var delimeter = "";
            foreach (var tupleMemberProxy in _tupleInfo.MethodProxies)
            {
                stringBuilder.Append(delimeter);
                stringBuilder.Append(tupleMemberProxy.Invoke(tupleValue, new object[] {}));
                delimeter = DELIMETER;
            }

            return stringBuilder.ToString();
        }

        internal TTuple DeserializeTuple(string stringValue, ICache<string, TTuple> cache)
        {
            return cache.GetOrAdd(stringValue, DeserializeTuple);
        }

        internal TTuple DeserializeTuple(string stringValue)
        {
            var stringValues = stringValue.Split(new[] { DELIMETER }, StringSplitOptions.None);

            if (stringValues.Length != _tupleInfo.Count)
            {
                throw new InvalidOperationException();
            }

            var objects = MapStringValuesToObjects(stringValues);

            return (TTuple) Activator.CreateInstance(typeof (TTuple), objects);
        }

        internal object[] MapStringValuesToObjects(string[] stringValues)
        {
            var objects = new object[_tupleInfo.Count];

            for (var i = 0; i < _tupleInfo.Count; i++)
            {
                var converter = TypeDescriptor.GetConverter(_tupleInfo.SubTypes[i]);
                var result = converter.ConvertFrom(stringValues[i]);
                objects[i] = result;
            }

            return objects;
        }
    }
}