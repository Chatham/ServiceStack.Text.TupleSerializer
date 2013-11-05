﻿using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ServiceStack.Text.TupleSerializer
{
    [ExcludeFromCodeCoverage]
    internal static class TupleSerializationCache<TTuple> where TTuple
        : IStructuralEquatable, IStructuralComparable, IComparable
    {
        public static ConcurrentDictionaryCache<TTuple, string> SerializeCache = new ConcurrentDictionaryCache<TTuple, string>();
        public static ConcurrentDictionaryCache<string, TTuple> DeserializeCache = new ConcurrentDictionaryCache<string, TTuple>(); 
    }
}