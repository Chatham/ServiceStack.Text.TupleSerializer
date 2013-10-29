﻿using System;
using ServiceStack.Text.InlineTupleSerializer.Api;

namespace ServiceStack.Text.InlineTupleSerializer.UnitTests
{
    public class PassThroughCache<TKey, TValue> : ICache<TKey, TValue>
    {
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            return valueFactory(key);
        }
    }
}
