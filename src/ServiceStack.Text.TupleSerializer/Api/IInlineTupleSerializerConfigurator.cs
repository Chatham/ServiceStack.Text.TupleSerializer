using System;
using System.Collections.Generic;
using System.Reflection;

namespace ServiceStack.Text.TupleSerializer.Api
{
    public interface IInlineTupleSerializerConfigurator
    {
        IInlineTupleSerializerConfigurator WithNamespaceFilter(Func<string, bool> enumNamespaceFilter);

        IInlineTupleSerializerConfigurator WithAssemblies(ICollection<Assembly> assembliesToScan);

        void Configure();
    }
}