using System;
using System.Collections.Generic;
using System.Threading;
using ServiceStack.Text.InlineTupleSerializer.Api;

namespace ServiceStack.Text.InlineTupleSerializer.PerformanceTests
{
    public class ReadWriterLockSlimDictionaryCache<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private readonly Dictionary<TKey, TValue> _cache = new Dictionary<TKey, TValue>();

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            _lock.EnterUpgradeableReadLock();
            try
            {
                TValue returnValue;
                if (_cache.TryGetValue(key, out returnValue))
                    return returnValue;

                _lock.EnterWriteLock();
                try
                {
                    returnValue = valueFactory(key);
                    _cache.Add(key, returnValue);
                    return returnValue;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }
        }
    }
}
