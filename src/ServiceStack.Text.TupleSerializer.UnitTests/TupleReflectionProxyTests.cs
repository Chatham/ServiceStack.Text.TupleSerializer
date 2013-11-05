using System;
using ExampleTuples;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceStack.Text.TupleSerializer.UnitTests
{
    [TestClass]
    public class TupleReflectionProxyTests
    {
        [TestMethod]
        public void Count_OnTupleType_ReturnsCorrectCount()
        {
            var info = new TupleReflectionProxy<Tuple<string, string>>();
            Assert.AreEqual(2, info.Count);
        }

        [TestMethod]
        public void Count_OnInheritedType_ReturnsCorrectCount()
        {
            var info = new TupleReflectionProxy<ObjectThatInheritsFromTuple>();
            Assert.AreEqual(2, info.Count); 
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Constructor_FakeTupleType_ThrowsException()
        {
            new TupleReflectionProxy<FakeTuple>();
        }
    }
}
