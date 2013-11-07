using System;
using System.Collections.Generic;
using ServiceStack.Text.TupleSerializer.Api;

namespace ServiceStack.Text.TupleSerializer.UnitTests
{
    public class TupleSerializerInitializerProxyFake : ITupleSerializerInitializerProxy
    {
        public List<Type> ConfigedTypes = new List<Type>();

        public void ConfigInlineTupleSerializer(Type type, string delimiter)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            ConfigedTypes.Add(type);
        }
    }
}