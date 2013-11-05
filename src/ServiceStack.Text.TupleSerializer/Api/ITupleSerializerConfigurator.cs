using System;
using System.Collections.Generic;
using System.Reflection;

namespace ServiceStack.Text.TupleSerializer.Api
{
    public interface ITupleSerializerConfigurator
    {
        ITupleSerializerConfigurator WithNamespaceFilter(Func<string, bool> enumNamespaceFilter);

        ITupleSerializerConfigurator WithAssemblies(ICollection<Assembly> assembliesToScan);

        void Configure();
    }
}