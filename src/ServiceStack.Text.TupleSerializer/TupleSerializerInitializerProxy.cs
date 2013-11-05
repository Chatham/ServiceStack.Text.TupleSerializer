using System;
using ServiceStack.Text.InlineTupleSerializer.Api;

namespace ServiceStack.Text.InlineTupleSerializer
{
    internal class TupleSerializerInitializerProxy : ITupleSerializerInitializerProxy
    {
        //Hide the static class interaction as much as possible
        public void ConfigInlineTupleSerializer(Type type)
        {
            Type enumHelperType = typeof(TupleSerializerInitializer<>).MakeGenericType(new[] { type });
            enumHelperType.CreateInstance();
        }
    }
}