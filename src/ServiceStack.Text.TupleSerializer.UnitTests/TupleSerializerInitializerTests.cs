using System;
using Xunit;

namespace ServiceStack.Text.TupleSerializer.UnitTests
{
    public class TupleSerializerInitializerTests
    {
        [Fact]
        public void Constructor_NonTupleObject_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new TupleSerializerInitializer<FakeTuple>(string.Empty));
        }
    }
}
