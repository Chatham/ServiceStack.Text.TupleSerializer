using System;

namespace TupleWithNamespace
{
    public class ObjectThatInheritsFromTuple : Tuple<string, string>
    {
        public ObjectThatInheritsFromTuple(string item1, string item2) : base(item1, item2)
        {
        }
    }

    public class ObjectWithTupleProperty
    {
        public Tuple<double, double> DoublePairOfDoubles { get; set; }
    }
}
