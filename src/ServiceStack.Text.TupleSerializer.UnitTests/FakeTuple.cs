using System;
using System.Collections;

namespace ServiceStack.Text.TupleSerializer.UnitTests
{
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