using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceStack.Text.InlineTupleSerializer.UnitTests
{
    [TestClass]
    public class TupleSerializerInitializerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NonTupleObject_ThrowsException()
        {
            new TupleSerializerInitializer<FakeTuple>();
        }

        internal class FakeTuple : IStructuralEquatable, IStructuralComparable, IComparable
        {
            public bool Equals(object other, IEqualityComparer comparer)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode(IEqualityComparer comparer)
            {
                throw new NotImplementedException();
            }

            public int CompareTo(object other, IComparer comparer)
            {
                throw new NotImplementedException();
            }

            public int CompareTo(object obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}
