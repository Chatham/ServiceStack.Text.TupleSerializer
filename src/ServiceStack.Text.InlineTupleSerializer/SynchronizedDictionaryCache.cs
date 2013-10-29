using System;
using System.Collections;
using System.Collections.Generic;
using ServiceStack.Text.InlineTupleSerializer.Api;

namespace ServiceStack.Text.InlineTupleSerializer
{
    public class SynchronizedDictionaryCache<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _cache = new Dictionary<TKey, TValue>();

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            TValue returnValue;
            if (_cache.TryGetValue(key, out returnValue))
                return returnValue;

            lock (((ICollection)_cache).SyncRoot)
            {
                if (_cache.TryGetValue(key, out returnValue))
                    return returnValue;

                returnValue = valueFactory(key);
                _cache.Add(key, returnValue);
                return returnValue;
            }
        }
    }
}
