using System;

namespace ServiceStack.Text.TupleSerializer.Api
{
    internal interface ITupleSerializerInitializerProxy
    {
        void ConfigInlineTupleSerializer(Type type, string delimeter);
    }
}