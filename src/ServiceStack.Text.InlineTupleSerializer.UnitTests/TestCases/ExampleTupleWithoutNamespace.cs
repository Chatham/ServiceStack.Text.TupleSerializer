using System;

class ExampleTupleWithoutNamespace : Tuple<int, string>
{
    public ExampleTupleWithoutNamespace(int item1, string item2) : base(item1, item2)
    {
    }
}