using System;
using System.Collections.Generic;
using System.Reflection;

namespace ServiceStack.Text.TupleSerializer.Api
{
    public interface ITupleSerializerConfigurator
    {
        ITupleSerializerConfigurator WithDelimiter(string delimiter);

        ITupleSerializerConfigurator WithNamespaceFilter(Func<string, bool> enumNamespaceFilter);

        ITupleSerializerConfigurator WithAssemblies(ICollection<Assembly> assembliesToScan);

        ITupleSerializerConfigurator WithTupleTypes(ICollection<Type> types);

        void Configure();
    }
}