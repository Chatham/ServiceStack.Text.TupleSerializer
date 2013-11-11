using System;
using System.Collections.Generic;
using System.Reflection;
using ServiceStack.Text.TupleSerializer.Api;

namespace ServiceStack.Text.TupleSerializer
{
    public sealed class TupleSerializerConfigurator : ITupleSerializerConfigurator
    {
        internal readonly HashSet<Assembly> _assembliesToScan = new HashSet<Assembly>();

        internal readonly HashSet<Type> _tupleTypes = new HashSet<Type>();

        internal string _delimiter;

        private Func<string, bool> _namespaceFilter;

        private ITupleSerializerInitializerProxy _jsConfigManager;

        internal ITupleSerializerInitializerProxy JsConfigProxy
        {
            get { return _jsConfigManager ?? (_jsConfigManager = new TupleSerializerInitializerProxy()); }
            set { _jsConfigManager = value; }
        }

        public ITupleSerializerConfigurator WithDelimiter(string delimiter)
        {
            _delimiter = delimiter;
            return this;
        }

        public ITupleSerializerConfigurator WithNamespaceFilter(Func<string, bool> namespaceFilter)
        {
            _namespaceFilter = namespaceFilter;
            return this;
        }

        public ITupleSerializerConfigurator WithAssemblies(ICollection<Assembly> assembliesToScan)
        {
            if (assembliesToScan.IsEmpty()) return this;

            foreach (var assembly in assembliesToScan)
            {
                if (assembly != null)
                {
                    _assembliesToScan.Add(assembly);
                }
            }

            return this;
        }

        public ITupleSerializerConfigurator WithTupleTypes(ICollection<Type> types)
        {
            if (!types.IsEmpty())
            {
                var publicTuples = types.GetTuples();
                _tupleTypes.UnionWith(publicTuples);
            }

            return this;
        }

        public void Configure()
        {
            var publicTuples = _assembliesToScan.GetPublicTuples(_namespaceFilter);
            publicTuples.UnionWith(_tupleTypes);
            foreach (var publicTuple in publicTuples)
            {
                JsConfigProxy.ConfigInlineTupleSerializer(publicTuple, _delimiter);
            }
        }
    }
}
