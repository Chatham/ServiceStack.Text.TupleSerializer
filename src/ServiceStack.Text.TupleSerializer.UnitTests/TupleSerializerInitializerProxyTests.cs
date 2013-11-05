using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceStack.Text.InlineTupleSerializer.UnitTests
{
    [TestClass]
    public class TupleSerializerInitializerProxyTests
    {
        [TestMethod]
        public void ConfigInlineTupleSerializer_TupleType_JsConfigFuncsSet()
        {
            // locking in case tests are multi threaded.
            lock (StaticTestingLocks.JsConfigLockObject)
            {
                JsConfig<Tuple<string, string>>.Reset();

                var proxy = new TupleSerializerInitializerProxy();
                proxy.ConfigInlineTupleSerializer(typeof(Tuple<string, string>));
                
                Assert.AreEqual("GetStringValue", JsConfig<Tuple<string, string>>.SerializeFn.Method.Name);
                Assert.AreEqual("GetTupleFrom", JsConfig<Tuple<string, string>>.DeSerializeFn.Method.Name);
            }
        }
    }
}
