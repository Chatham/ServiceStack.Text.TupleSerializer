using System;

namespace ServiceStack.Text.InlineTupleSerializer.UnitTests
{
    public class StringPair : Tuple<string, string>
    {
        public StringPair(string item1, string item2) : base(item1, item2)
        {
        }
    }

    public interface IGetReturn<TResponse>
    {
    }

    public class ResponseObject : IGetReturn<StringPair>
    {
    }
}
