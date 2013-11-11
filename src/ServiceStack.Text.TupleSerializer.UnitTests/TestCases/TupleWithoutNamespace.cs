using System;

class TupleWithoutNamespace : Tuple<int, string>
{
    public TupleWithoutNamespace(int item1, string item2) : base(item1, item2)
    {
    }
}