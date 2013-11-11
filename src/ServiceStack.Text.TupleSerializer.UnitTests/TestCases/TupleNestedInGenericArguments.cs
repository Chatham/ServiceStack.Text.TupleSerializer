using System;
using System.Collections;
using System.Collections.Generic;

namespace NestedInGenerics
{
    public class ObjectWithTupleNestedInDefinition_ImplementedGenericInterface : IEnumerable<Tuple<string>>
    {
        public IEnumerator<Tuple<string>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ObjectWithTupleNestedInDefinition_InheritedGenericClass : List<Tuple<int>>
    {
    }

    public class ObjectWithTupleNestedInProperty_GenericClass
    {
        public List<Tuple<bool, bool>> PairOfBools { get; set; }
    }

    public class ObjectWithTupleNestedInProperty_GenericInterface
    {
        public IList<Tuple<int, int>> PairOfIntegers { get; set; }
    }
}
