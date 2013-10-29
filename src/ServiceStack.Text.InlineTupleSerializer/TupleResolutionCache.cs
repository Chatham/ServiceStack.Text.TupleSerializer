using System;
using System.Collections.Concurrent;

namespace ServiceStack.Text.InlineTupleSerializer
{
    internal static class TupleResolutionCache
    {
        public static ConcurrentDictionary<Type, bool> IsTupleCache = new ConcurrentDictionary<Type, bool>(); 
    }
}