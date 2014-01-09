using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using ServiceStack.Text.TupleSerializer.Api;

namespace ServiceStack.Text.TupleSerializer
{
    [ExcludeFromCodeCoverage]
    internal class ConcurrentDictionaryCache<TKey, TValue> : ConcurrentDictionary<TKey, TValue>, ICache<TKey, TValue>
    {
    }
}