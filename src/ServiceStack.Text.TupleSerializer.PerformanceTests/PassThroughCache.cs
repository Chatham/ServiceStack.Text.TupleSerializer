using System;
using ServiceStack.Text.TupleSerializer.Api;

namespace ServiceStack.Text.TupleSerializer.PerformanceTests
{
    public class PassThroughCache<TKey, TValue> : ICache<TKey, TValue>
    {
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            return valueFactory(key);
        }
    }
}
