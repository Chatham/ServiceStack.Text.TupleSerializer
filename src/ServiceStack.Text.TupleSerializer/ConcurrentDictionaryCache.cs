using System.Collections.Concurrent;
using ServiceStack.Text.TupleSerializer.Api;

namespace ServiceStack.Text.TupleSerializer
{
    internal class ConcurrentDictionaryCache<TKey, TValue> : ConcurrentDictionary<TKey, TValue>, ICache<TKey, TValue>
    {
    }
}