using System;

namespace ServiceStack.Text.InlineTupleSerializer.Api
{
    internal interface ICache<TKey, TValue>
    {
        TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);
    }
}