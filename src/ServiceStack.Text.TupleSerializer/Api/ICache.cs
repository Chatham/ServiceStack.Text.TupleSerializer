using System;

namespace ServiceStack.Text.TupleSerializer.Api
{
    internal interface ICache<TKey, TValue>
    {
        TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);
    }
}