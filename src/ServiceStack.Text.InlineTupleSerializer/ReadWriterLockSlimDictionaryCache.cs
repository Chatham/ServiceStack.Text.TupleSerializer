using System;
using System.Collections.Generic;
using System.Threading;
using ServiceStack.Text.InlineTupleSerializer.Api;

namespace ServiceStack.Text.InlineTupleSerializer
{
    public class ReadWriterLockSlimDictionaryCache<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        private readonly Dictionary<TKey, TValue> innerCache = new Dictionary<TKey, TValue>();

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            TValue returnValue;
            if (TryGetValue(key, out returnValue))
            {
                return returnValue;
            }
            return Add(key, valueFactory);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            cacheLock.EnterReadLock();
            try
            {
                return innerCache.TryGetValue(key, out value);
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

        public TValue Add(TKey key, Func<TKey, TValue> valueFactory)
        {
            cacheLock.EnterWriteLock();
            try
            {
                var value = valueFactory(key);
                innerCache.Add(key, value);
                return value;
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }
    }
}
