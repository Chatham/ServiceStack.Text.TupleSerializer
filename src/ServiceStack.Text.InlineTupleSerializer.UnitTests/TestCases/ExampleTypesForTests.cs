using System;

namespace ServiceStack.Text.InlineTupleSerializer.UnitTests.TestCases
{
    public class StringPair : Tuple<string, string>
    {
        public StringPair(string item1, string item2) : base(item1, item2)
        {
        }
    }

    public class StringTriad : Tuple<string, string, string>
    {
        public StringTriad(string item1, string item2, string item3)
            : base(item1, item2, item3)
        {
        }
    }

    public interface IGetReturn<TResponse>
    {
    }

    public class ResponseObjectWithInheritedTuple : IGetReturn<StringTriad>
    {
    }

    public class ResponseObjectWithVanillaTuple : IGetReturn<Tuple<string>>
    {
    }

    public class RequestObjectWithInheritedTuple
    {
        public StringPair StringPair { get; set; }
    }
}
