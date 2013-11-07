using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using ServiceStack.Text.TupleSerializer.Api;

namespace ServiceStack.Text.TupleSerializer
{
    internal class TupleSerializer<TTuple> : ITupleSerializer<TTuple> 
        where TTuple : IStructuralEquatable, IStructuralComparable, IComparable
    {
        internal string _delimeter = "-";

        internal readonly TupleReflectionProxy<TTuple> _tupleInfo;
        internal readonly ICache<TTuple, string> _serializationCache;
        internal readonly ICache<string, TTuple> _deserializationCache;

        public TupleSerializer()
            : this(new TupleReflectionProxy<TTuple>(), new ConcurrentDictionaryCache<TTuple, string>(), new ConcurrentDictionaryCache<string, TTuple>())
        {
        }

        public TupleSerializer(
            ICache<TTuple, string> serializationCache,
            ICache<string, TTuple> deserializationCache)
            : this(new TupleReflectionProxy<TTuple>(), serializationCache, deserializationCache)
        {
        }

        public TupleSerializer(
            TupleReflectionProxy<TTuple> tupleInfo,
            ICache<TTuple, string> serializationCache,
            ICache<string, TTuple> deserializationCache)
        {
            _tupleInfo = tupleInfo;
            _serializationCache = serializationCache; 
            _deserializationCache = deserializationCache;
        }

        public TupleSerializer<TTuple> SetDelimeter(string delimeter)
        {
            if (!string.IsNullOrEmpty(delimeter))
            {
                _delimeter = delimeter;
            }
            return this;
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
                delimeter = _delimeter;
            }

            return stringBuilder.ToString();
        }

        internal TTuple DeserializeTuple(string stringValue, ICache<string, TTuple> cache)
        {
            return cache.GetOrAdd(stringValue, DeserializeTuple);
        }

        internal TTuple DeserializeTuple(string stringValue)
        {
            var stringValues = stringValue.Split(new[] { _delimeter }, StringSplitOptions.None);

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