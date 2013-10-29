using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceStack.Text.InlineTupleSerializer.UnitTests
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
            var info = new TupleReflectionProxy<StringPair>();
            Assert.AreEqual(2, info.Count); 
        }
    }
}
