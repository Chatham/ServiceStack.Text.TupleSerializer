using System;
using TupleWithNamespace;
using Xunit;

namespace ServiceStack.Text.TupleSerializer.UnitTests
{
    public class TupleReflectionProxyTests
    {
        [Fact]
        public void Count_OnTupleType_ReturnsCorrectCount()
        {
            var info = new TupleReflectionProxy<Tuple<string, string>>();
            Assert.Equal(2, info.Count);
        }

        [Fact]
        public void Count_OnInheritedType_ReturnsCorrectCount()
        {
            var info = new TupleReflectionProxy<ObjectThatInheritsFromTuple>();
            Assert.Equal(2, info.Count); 
        }

        [Fact]
        public void Constructor_FakeTupleType_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => new TupleReflectionProxy<FakeTuple>());
        }
    }
}
