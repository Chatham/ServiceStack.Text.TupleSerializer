using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceStack.Text.InlineTupleSerializer.UnitTests
{
    [TestClass]
    public class CollectionExtensionsTests
    {
        [TestMethod]
        public void GetTuples_NullCollection_ReturnsEmptyHashSet()
        {
            var hashSet = CollectionExtensions.GetTuples(null);
            Assert.IsTrue(hashSet.IsEmpty());
        }

        [TestMethod]
        public void GetTuples_EmptyCollection_ReturnsEmptyHashSet()
        {
            var hashSet = new List<Type>().GetTuples();
            Assert.IsTrue(hashSet.IsEmpty());
        }
    }
}
