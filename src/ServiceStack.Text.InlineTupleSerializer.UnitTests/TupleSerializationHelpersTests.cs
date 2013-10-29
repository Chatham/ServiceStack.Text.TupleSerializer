using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceStack.Text.InlineTupleSerializer.UnitTests
{
    [TestClass]
    public class TupleSerializationHelpersTests
    {
        [TestMethod]
        public void Serialize()
        {
            var sh = new TupleSerializationHelpers<Tuple<string, string, string>>();

            var ser = sh.GetStringValue(new Tuple<string, string, string>("EUR", "EUR", "EUR"));

            Assert.AreEqual("EUR-EUR-EUR", ser);
        }

        [TestMethod]
        public void Deserialize()
        {
            var sh = new TupleSerializationHelpers<Tuple<string, string, string>>();

            var ser = sh.GetTupleFrom("EUR-EUR-EUR");

            Assert.AreEqual(new Tuple<string, string, string>("EUR", "EUR", "EUR"), ser);
        }

        [TestMethod]
        public void Deserialize_Inherited()
        {
            var sh = new TupleSerializationHelpers<StringPair>();

            var ser = sh.GetTupleFrom("EUR-EUR");

            Assert.AreEqual(new StringPair("EUR", "EUR"), ser);
        }
    }
}
