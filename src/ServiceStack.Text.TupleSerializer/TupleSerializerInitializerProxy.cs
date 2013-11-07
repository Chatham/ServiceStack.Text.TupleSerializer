using System;
using ServiceStack.Text.TupleSerializer.Api;

namespace ServiceStack.Text.TupleSerializer
{
    internal class TupleSerializerInitializerProxy : ITupleSerializerInitializerProxy
    {
        //Hide the static class interaction as much as possible
        public void ConfigInlineTupleSerializer(Type type, string delimiter)
        {
            Type enumHelperType = typeof(TupleSerializerInitializer<>).MakeGenericType(new[] { type });
            Activator.CreateInstance(enumHelperType, new object[] {delimiter});
        }
    }
}