using System;
using ExampleTuples;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceStack.Text.InlineTupleSerializer.UnitTests
{
    [TestClass]
    public class TypeExtensionsTests
    {
        [TestMethod]
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
                Assert.IsTrue(tuple.GetType().IsTuple());
            }
        }

        [TestMethod]
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
                Assert.IsTrue(tupleType.IsTuple());
            }
        }

        [TestMethod]
        public void IsTuple_ForRegularType_ReturnsFalse()
        {
            Assert.IsFalse(typeof (String).IsTuple());
        }

        [TestMethod]
        public void IsTuple_ForInheritedType_ReturnsTrue()
        {
            Assert.IsTrue(typeof(ObjectThatInheritsFromTuple).IsTuple());
        }

        [TestMethod]
        public void FindTupleDefinition_NonTupleType_ReturnsNull()
        {
            var type = typeof(string).FindTupleDefinition();
            Assert.IsNull(type);
        }

        [TestMethod]
        public void FindTupleDefinition_GenericTupleTypeDefinition_ReturnsNull()
        {
            var type = typeof (Tuple<,>).FindTupleDefinition();
            Assert.IsNull(type);
        }

        [TestMethod]
        public void FindTupleDefinition_ConcreteTupleTypeDefinition_ReturnsUnderlyingConcreteTupleType()
        {
            var type = typeof(Tuple<string, string>).FindTupleDefinition();
            Assert.AreEqual(typeof(Tuple<string,string>), type);
        }

        [TestMethod]
        public void FindTupleDefinition_InheritedTupleTypeDefinition_ReturnsUnderlyingConcreteTupleType()
        {
            var type = typeof(ObjectThatInheritsFromTuple).FindTupleDefinition();
            Assert.AreEqual(typeof(Tuple<string, string>), type);
        }

        [TestMethod]
        public void FindTupleDefinition_ObjectType_ReturnsNull()
        {
            var type = typeof(Object).FindTupleDefinition();
            Assert.IsNull(type);
        }

        [TestMethod]
        public void FindTupleDefinition_Null_ReturnsNull()
        {
            Assert.IsNull(TypeExtensions.FindTupleDefinition(null));
        }
    }
}
