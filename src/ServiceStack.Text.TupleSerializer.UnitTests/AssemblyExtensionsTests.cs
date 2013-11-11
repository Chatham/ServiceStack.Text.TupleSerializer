using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace ServiceStack.Text.TupleSerializer.UnitTests
{
    public class AssemblyExtensionsTests
    {
        [Fact]
        public void AlwaysTrueFilter_WhenInvoked_ReturnsTrue()
        {
            Assert.True(AssemblyExtensions.AlwaysTrueFilter(string.Empty));
        }

        [Fact]
        public void GetPublicTuples_NullCollection_ReturnEmptyHashSet()
        {
            var hashSet = AssemblyExtensions.GetPublicTuples(null, null);
            Assert.Empty(hashSet);
        }

        [Fact]
        public void GetPublicTuples_EmptyCollection_ReturnEmptyHashSet()
        {
            var hashSet = new List<Assembly>().GetPublicTuples(null);
            Assert.Empty(hashSet);
        }

        [Fact]
        public void GetPublicTuples_CollectionWithNullAssembly_ShouldSkipInspectingNullAssembly()
        {
            var executionCount = 0;
            Func<string, bool> ExecutionCountFilter = s => {
                executionCount++;
                return true;
            };

            new List<Assembly> { null }.GetPublicTuples(ExecutionCountFilter);

            Assert.Equal(0, executionCount);
        }
    }
}
