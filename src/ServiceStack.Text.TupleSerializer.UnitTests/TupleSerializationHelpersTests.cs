using System;
using ExampleTuples;
using Rhino.Mocks;
using Xunit;

namespace ServiceStack.Text.TupleSerializer.UnitTests
{
    public class TupleSerializationHelpersTests
    {
        [Fact]
        public void Serialize()
        {
            var sh = new TupleSerializationHelpers<Tuple<string, string, string>>();

            var ser = sh.GetStringValue(new Tuple<string, string, string>("EUR", "EUR", "EUR"));

            Assert.Equal("EUR-EUR-EUR", ser);
        }

        [Fact]
        public void Deserialize()
        {
            var sh = new TupleSerializationHelpers<Tuple<string, string, string>>();

            var ser = sh.GetTupleFrom("EUR-EUR-EUR");

            Assert.Equal(new Tuple<string, string, string>("EUR", "EUR", "EUR"), ser);
        }

        [Fact]
        public void Deserialize_Inherited()
        {
            var sh = new TupleSerializationHelpers<ObjectThatInheritsFromTuple>();

            var ser = sh.GetTupleFrom("EUR-EUR");

            Assert.Equal(new ObjectThatInheritsFromTuple("EUR", "EUR"), ser);
        }

        [Fact]
        public void Deserialize_TuplePairSerializerGetsTupleTriad_ThrowsException()
        {
            var sh = new TupleSerializationHelpers<Tuple<string, string>>();
            Assert.Throws<InvalidOperationException>(() => sh.GetTupleFrom("EUR-EUR-EUR"));
        }

        [Fact]
        public void Constructor_CacheInjection_SetsInternalCacheReferences()
        {
            var serCache = MockRepository.GenerateStub<ConcurrentDictionaryCache<Tuple<string>, string>>();
            var deSerCache = MockRepository.GenerateStub<ConcurrentDictionaryCache<string, Tuple<string>>>();

            var ser = new TupleSerializationHelpers<Tuple<string>>(serCache, deSerCache);

            Assert.Equal(ser._serializationCache, serCache);
            Assert.Equal(ser._deserializationCache, deSerCache);
        }
    }
}
