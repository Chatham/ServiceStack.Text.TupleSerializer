using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using ServiceStack.Text.InlineTupleSerializer.Api;

namespace ServiceStack.Text.InlineTupleSerializer
{
    [ExcludeFromCodeCoverage]
    public class ConcurrentDictionaryCache<TKey, TValue> : ConcurrentDictionary<TKey, TValue>, ICache<TKey, TValue>
    {
    }
}