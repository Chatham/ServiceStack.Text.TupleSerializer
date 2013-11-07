using System;
using ExampleTuples;
using Rhino.Mocks;
using Xunit;

namespace ServiceStack.Text.TupleSerializer.UnitTests
{
    public class TupleSerializerTests
    {
        [Fact]
        public void GetStringValue_Triad_ReturnsSerializedStringUsingDefaultDelimeter()
        {
            var ser = new TupleSerializer<Tuple<string, string, string>>();

            var str = ser.GetStringValue(new Tuple<string, string, string>("EUR", "EUR", "EUR"));

            Assert.Equal("EUR-EUR-EUR", str);
        }

        [Fact]
        public void GetStringValue_TriadWithCustomDelimeter_ReturnsSerializedStringUsingCustomDelimeter()
        {
            var ser = new TupleSerializer<Tuple<string, string, string>>()
                .SetDelimeter(":");

            var str = ser.GetStringValue(new Tuple<string, string, string>("EUR", "EUR", "EUR"));

            Assert.Equal("EUR:EUR:EUR", str);
        }

        [Fact]
        public void SetDelimeter_ValidCustomDelimeter_SetsCustomDelimeter()
        {
            var ser = new TupleSerializer<Tuple<string, string, string>>()
                .SetDelimeter(":");
            Assert.Equal(":", ser._delimeter);
        }

        [Fact]
        public void SetDelimeter_InvalidCustomDelimeter_DoesNotSetCustomDelimeter()
        {
            var ser = new TupleSerializer<Tuple<string, string, string>>()
                .SetDelimeter(string.Empty);
            Assert.Equal("-", ser._delimeter);
        }

        [Fact]
        public void GetTupleFrom_StringRepresentingTriad_ReturnsDeserializedTriad()
        {
            var ser = new TupleSerializer<Tuple<string, string, string>>();

            var tuple = ser.GetTupleFrom("EUR-EUR-EUR");

            Assert.Equal(new Tuple<string, string, string>("EUR", "EUR", "EUR"), tuple);
        }

        [Fact]
        public void Constructor_NestedTupleType_ThrowsException()
        {
            Assert.Throws<NotImplementedException>(() => new TupleSerializer<Tuple<Tuple<string, string>, string, string>>());
        }

        [Fact]
        public void GetTupleFrom_StringRepresentingTriadWithCustomDelimeter_ReturnsDeserializedTriad()
        {
            var ser = new TupleSerializer<Tuple<string, string, string>>()
                .SetDelimeter(":");

            var tuple = ser.GetTupleFrom("EUR:EUR:EUR");

            Assert.Equal(new Tuple<string, string, string>("EUR", "EUR", "EUR"), tuple);
        }

        [Fact]
        public void GetTupleFrom_StringRepresentingPair_DeserializesToInheritedObject()
        {
            var ser = new TupleSerializer<ObjectThatInheritsFromTuple>();

            var tuple = ser.GetTupleFrom("EUR-EUR");

            Assert.Equal(new ObjectThatInheritsFromTuple("EUR", "EUR"), tuple);
        }

        [Fact]
        public void GetTupleFrom_TuplePairSerializerGetsTupleTriad_ThrowsException()
        {
            var ser = new TupleSerializer<Tuple<string, string>>();
            Assert.Throws<InvalidOperationException>(() => ser.GetTupleFrom("EUR-EUR-EUR"));
        }

        [Fact]
        public void Constructor_CacheInjection_SetsInternalCacheReferences()
        {
            var serCache = MockRepository.GenerateStub<ConcurrentDictionaryCache<Tuple<string>, string>>();
            var deSerCache = MockRepository.GenerateStub<ConcurrentDictionaryCache<string, Tuple<string>>>();

            var ser = new TupleSerializer<Tuple<string>>(serCache, deSerCache);

            Assert.Equal(ser._serializationCache, serCache);
            Assert.Equal(ser._deserializationCache, deSerCache);
        }
    }
}
