using System;
using Xunit;

namespace ServiceStack.Text.TupleSerializer.UnitTests
{
    public class TupleSerializerInitializerProxyTests
    {
        [Fact]
        public void ConfigInlineTupleSerializer_TupleType_JsConfigFuncsSet()
        {
            // locking in case tests are multi threaded.
            lock (StaticTestingLocks.JsConfigLockObject)
            {
                JsConfig<Tuple<string, string>>.Reset();

                var proxy = new TupleSerializerInitializerProxy();
                proxy.ConfigInlineTupleSerializer(typeof(Tuple<string, string>));
                
                Assert.Equal("GetStringValue", JsConfig<Tuple<string, string>>.SerializeFn.Method.Name);
                Assert.Equal("GetTupleFrom", JsConfig<Tuple<string, string>>.DeSerializeFn.Method.Name);
            }
        }
    }
}
