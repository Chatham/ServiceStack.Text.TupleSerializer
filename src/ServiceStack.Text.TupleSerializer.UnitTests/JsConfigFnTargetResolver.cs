using System;

namespace ServiceStack.Text.TupleSerializer.UnitTests
{
    internal static class JsConfigFnTargetResolver<T>
    {
        public static Func<string, T> GetDeserializer()
        {
            return GetDeserializer("DeSerializeFn");
        }

        private static Func<string, T> GetDeserializer(string name)
        {
            var field = typeof(JsConfig<T>).GetField(name);
            object value;
            if (field != null)
            {
                value = field.GetValue(null);
            }
            else
            {
                var property = typeof(JsConfig<T>).GetProperty(name);
                value = property.GetValue(null, null);
            }

            return (Func<string, T>)value;
        }
    }
}