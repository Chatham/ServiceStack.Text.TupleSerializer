using System.Collections.Concurrent;
using ServiceStack.Text.InlineTupleSerializer.Api;

namespace ServiceStack.Text.InlineTupleSerializer
{
    public class ConcurrentDictionaryCache<TKey, TValue> : ConcurrentDictionary<TKey, TValue>, ICache<TKey, TValue>
    {
    }
}