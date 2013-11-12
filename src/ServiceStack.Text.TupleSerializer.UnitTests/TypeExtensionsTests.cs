using System;
using System.Collections.Generic;
using System.Linq;
using TupleWithNamespace;
using Xunit;

namespace ServiceStack.Text.TupleSerializer.UnitTests
{
    public class TypeExtensionsTests
    {
        [Fact]
        public void IsTuple_ForEachConcreteTuple_ReturnsTrue()
        {
            var tuples = new object[]
            {
                new Tuple<int>(1),
                new Tuple<int, int>(1, 2),
                new Tuple<int, int, int>(1, 2, 3),
                new Tuple<int, int, int, int>(1, 2, 3, 4),
                new Tuple<int, int, int, int, int>(1, 2, 3, 4, 5),
                new Tuple<int, int, int, int, int, int>(1, 2, 3, 4, 5, 6),
                new Tuple<int, int, int, int, int, int, int>(1, 2, 3, 4, 5, 6, 7),
                new Tuple<int, int, int, int, int, int, int, Tuple<int>>(1, 2, 3, 4, 5, 6, 7, new Tuple<int>(8)),
            };
            
            foreach (var tuple in tuples)
            {
                Assert.True(tuple.GetType().IsTuple());
            }
        }

        [Fact]
        public void IsTuple_ForEachGenericTupleType_ReturnsTrue()
        {
            var tupleTypes = new[] {
                typeof (Tuple<>),
                typeof (Tuple<,>),
                typeof (Tuple<,,>),
                typeof (Tuple<,,,>),
                typeof (Tuple<,,,,>),
                typeof (Tuple<,,,,,>),
                typeof (Tuple<,,,,,,>),
                typeof (Tuple<,,,,,,,>),
            };

            foreach (var tupleType in tupleTypes)
            {
                Assert.True(tupleType.IsTuple());
            }
        }

        [Fact]
        public void IsTuple_ForRegularType_ReturnsFalse()
        {
            Assert.False(typeof (String).IsTuple());
        }

        [Fact]
        public void IsTuple_ForInheritedType_ReturnsTrue()
        {
            Assert.True(typeof(ObjectThatInheritsFromTuple).IsTuple());
        }

        [Fact]
        public void FindTupleDefinition_NonTupleType_ReturnsNull()
        {
            var type = typeof(string).FindTupleDefinition();
            Assert.Null(type);
        }

        [Fact]
        public void FindTupleDefinition_GenericTupleTypeDefinition_ReturnsNull()
        {
            var type = typeof (Tuple<,>).FindTupleDefinition();
            Assert.Null(type);
        }

        [Fact]
        public void FindTupleDefinition_ConcreteTupleTypeDefinition_ReturnsUnderlyingConcreteTupleType()
        {
            var type = typeof(Tuple<string, string>).FindTupleDefinition();
            Assert.Equal(typeof(Tuple<string,string>), type);
        }

        [Fact]
        public void FindTupleDefinition_InheritedTupleTypeDefinition_ReturnsUnderlyingConcreteTupleType()
        {
            var type = typeof(ObjectThatInheritsFromTuple).FindTupleDefinition();
            Assert.Equal(typeof(Tuple<string, string>), type);
        }

        [Fact]
        public void FindTupleDefinition_ObjectType_ReturnsNull()
        {
            var type = typeof(Object).FindTupleDefinition();
            Assert.Null(type);
        }

        [Fact]
        public void FindTupleDefinition_Null_ReturnsNull()
        {
            Assert.Null(TypeExtensions.FindTupleDefinition(null));
        }

        [Fact]
        public void GetTuples_NullCollection_ReturnsEmptyHashSet()
        {
            var hashSet = TypeExtensions.GetTuples(null);
            Assert.Empty(hashSet);
        }

        [Fact]
        public void GetTuples_EmptyCollection_ReturnsEmptyHashSet()
        {
            var hashSet = new List<Type>().GetTuples();
            Assert.Empty(hashSet);
        }

        [Fact]
        public void EnumerateTypeTrees_NullEnumerable_ReturnsEmptyEnumerator()
        {
            var count = TypeExtensions.EnumerateTypeTrees(null).Count();
            Assert.Equal(0, count);
        }
    }
}
