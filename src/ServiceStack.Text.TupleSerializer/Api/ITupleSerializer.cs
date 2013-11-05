using System;
using System.Collections;

namespace ServiceStack.Text.InlineTupleSerializer.Api
{
    internal interface ITupleSerializer<TTuple> 
        where TTuple : IStructuralEquatable, IStructuralComparable, IComparable
    {
        TTuple GetTupleFrom(string stringValue);
        string GetStringValue(TTuple tupleValue);
    }
}