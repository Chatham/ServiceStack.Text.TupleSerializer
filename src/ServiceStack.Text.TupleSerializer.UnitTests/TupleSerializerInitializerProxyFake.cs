using System;
using System.Collections.Generic;
using ServiceStack.Text.InlineTupleSerializer.Api;

namespace ServiceStack.Text.InlineTupleSerializer.UnitTests
{
    public class TupleSerializerInitializerProxyFake : ITupleSerializerInitializerProxy
    {
        public List<Type> ConfigedTypes = new List<Type>();

        public void ConfigInlineTupleSerializer(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            ConfigedTypes.Add(type);
        }
    }
}