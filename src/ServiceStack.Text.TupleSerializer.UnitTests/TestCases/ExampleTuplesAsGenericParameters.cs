using System;

namespace GenericParameters
{
    public interface IGetReturn<TResponse>
    {
    }

    public class ObjectThatInheritsFromGenericInterface : IGetReturn<Tuple<string>>
    {
    }

    public class ReturnBase<TType>
    {
    }

    public class ObjectThatInheritsFromGenericClass : ReturnBase<Tuple<int>>
    {
    }
}
