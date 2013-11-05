using System;
using ExampleTuples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace ServiceStack.Text.TupleSerializer.UnitTests
{
    [TestClass]
    public class TupleSerializationHelpersTests
    {
        [TestMethod]
        public void Serialize()
        {
            var sh = new TupleSerializationHelpers<Tuple<string, string, string>>();

            var ser = sh.GetStringValue(new Tuple<string, string, string>("EUR", "EUR", "EUR"));

            Assert.AreEqual("EUR-EUR-EUR", ser);
        }

        [TestMethod]
        public void Deserialize()
        {
            var sh = new TupleSerializationHelpers<Tuple<string, string, string>>();

            var ser = sh.GetTupleFrom("EUR-EUR-EUR");

            Assert.AreEqual(new Tuple<string, string, string>("EUR", "EUR", "EUR"), ser);
        }

        [TestMethod]
        public void Deserialize_Inherited()
        {
            var sh = new TupleSerializationHelpers<ObjectThatInheritsFromTuple>();

            var ser = sh.GetTupleFrom("EUR-EUR");

            Assert.AreEqual(new ObjectThatInheritsFromTuple("EUR", "EUR"), ser);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Deserialize_TuplePairSerializerGetsTupleTriad_ThrowsException()
        {
            var sh = new TupleSerializationHelpers<Tuple<string, string>>();

            var ser = sh.GetTupleFrom("EUR-EUR-EUR");
        }

        [TestMethod]
        public void Constructor_CacheInjection_SetsInternalCacheReferences()
        {
            var serCache = MockRepository.GenerateStub<ConcurrentDictionaryCache<Tuple<string>, string>>();
            var deSerCache = MockRepository.GenerateStub<ConcurrentDictionaryCache<string, Tuple<string>>>();

            var ser = new TupleSerializationHelpers<Tuple<string>>(serCache, deSerCache);

            Assert.AreEqual(ser._serializationCache, serCache);
            Assert.AreEqual(ser._deserializationCache, deSerCache);
        }
    }
}
