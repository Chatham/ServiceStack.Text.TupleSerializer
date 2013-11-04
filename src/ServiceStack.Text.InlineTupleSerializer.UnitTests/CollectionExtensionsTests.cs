using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceStack.Text.InlineTupleSerializer.UnitTests
{
    [TestClass]
    public class CollectionExtensionsTests
    {
        [TestMethod]
        public void GetPublicTuples_NullCollection_ReturnsEmptyHashSet()
        {
            var hashSet = CollectionExtensions.GetPublicTuples(null);
            Assert.IsTrue(hashSet.IsEmpty());
        }

        [TestMethod]
        public void GetPublicTuples_EmptyCollection_ReturnsEmptyHashSet()
        {
            var hashSet = new List<Type>().GetPublicTuples();
            Assert.IsTrue(hashSet.IsEmpty());
        }
    }
}
