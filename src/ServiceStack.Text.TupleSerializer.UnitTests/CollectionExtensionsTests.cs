using System;
using System.Collections.Generic;
using Xunit;

namespace ServiceStack.Text.TupleSerializer.UnitTests
{
    public class CollectionExtensionsTests
    {
        [Fact]
        public void GetTuples_NullCollection_ReturnsEmptyHashSet()
        {
            var hashSet = CollectionExtensions.GetTuples(null);
            Assert.Empty(hashSet);
        }

        [Fact]
        public void GetTuples_EmptyCollection_ReturnsEmptyHashSet()
        {
            var hashSet = new List<Type>().GetTuples();
            Assert.Empty(hashSet);
        }
    }
}
