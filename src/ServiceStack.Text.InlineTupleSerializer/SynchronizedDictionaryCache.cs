using System;
using System.Collections;
using System.Collections.Generic;
using ServiceStack.Text.InlineTupleSerializer.Api;

namespace ServiceStack.Text.InlineTupleSerializer
{
    public class SynchronizedDictionaryCache<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> innerCache = new Dictionary<TKey, TValue>();

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            TValue returnValue;
            if (innerCache.TryGetValue(key, out returnValue))
                return returnValue;

            lock (((ICollection)innerCache).SyncRoot)
            {
                if (innerCache.TryGetValue(key, out returnValue))
                    return returnValue;

                returnValue = valueFactory(key);
                innerCache.Add(key, returnValue);
                return returnValue;
            }
        }
    }
}
