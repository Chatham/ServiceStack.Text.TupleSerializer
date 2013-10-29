using System;
using System.Collections;
using System.Collections.Concurrent;

namespace ServiceStack.Text.InlineTupleSerializer
{
    internal static class InlineTupleSerializationCache<TTuple> where TTuple
        : IStructuralEquatable, IStructuralComparable, IComparable
    {
        public static ConcurrentDictionary<TTuple, string> SerializeCache = new ConcurrentDictionary<TTuple, string>();
        public static ConcurrentDictionary<string, TTuple> DeserializeCache = new ConcurrentDictionary<string, TTuple>(); 
    }
}