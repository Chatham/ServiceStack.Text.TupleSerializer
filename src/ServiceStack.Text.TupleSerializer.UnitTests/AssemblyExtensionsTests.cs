using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceStack.Text.InlineTupleSerializer.UnitTests
{
    [TestClass]
    public class AssemblyExtensionsTests
    {
        [TestMethod]
        public void AlwaysTrueFilter_WhenInvoked_ReturnsTrue()
        {
            Assert.IsTrue(AssemblyExtensions.AlwaysTrueFilter(string.Empty));
        }

        [TestMethod]
        public void GetPublicTuples_NullCollection_ReturnEmptyHashSet()
        {
            var hashSet = AssemblyExtensions.GetPublicTuples(null, null);
            Assert.IsTrue(hashSet.IsEmpty());
        }

        [TestMethod]
        public void GetPublicTuples_EmptyCollection_ReturnEmptyHashSet()
        {
            var hashSet = new List<Assembly>().GetPublicTuples(null);
            Assert.IsTrue(hashSet.IsEmpty());
        }

        [TestMethod]
        public void GetPublicTuples_CollectionWithNullAssembly_ShouldSkipInspectingNullAssembly()
        {
            var executionCount = 0;
            Func<string, bool> ExecutionCountFilter = s => {
                executionCount++;
                return true;
            };

            new List<Assembly> { null }.GetPublicTuples(ExecutionCountFilter);

            Assert.AreEqual(0, executionCount);
        }
    }
}
