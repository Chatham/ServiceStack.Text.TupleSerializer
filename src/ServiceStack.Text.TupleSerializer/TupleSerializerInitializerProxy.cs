using System;
using ServiceStack.Text.TupleSerializer.Api;

namespace ServiceStack.Text.TupleSerializer
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