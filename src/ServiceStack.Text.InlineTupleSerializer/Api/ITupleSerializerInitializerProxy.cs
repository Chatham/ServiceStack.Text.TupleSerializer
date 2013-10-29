using System;

namespace ServiceStack.Text.InlineTupleSerializer.Api
{
    internal interface ITupleSerializerInitializerProxy
    {
        void ConfigInlineTupleSerializer(Type type);
    }
}