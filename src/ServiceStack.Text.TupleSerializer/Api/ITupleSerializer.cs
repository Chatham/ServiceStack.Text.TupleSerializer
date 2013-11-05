using System;
using System.Collections;

namespace ServiceStack.Text.TupleSerializer.Api
{
    internal interface ITupleSerializer<TTuple> 
        where TTuple : IStructuralEquatable, IStructuralComparable, IComparable
    {
        TTuple GetTupleFrom(string stringValue);
        string GetStringValue(TTuple tupleValue);
    }
}