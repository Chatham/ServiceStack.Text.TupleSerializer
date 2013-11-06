using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ServiceStack.Text.TupleSerializer.Api;

namespace ServiceStack.Text.TupleSerializer
{
    public sealed class TupleSerializerConfigurator : ITupleSerializerConfigurator
    {
        internal readonly HashSet<Assembly> _assembliesToScan = new HashSet<Assembly>();

        private Func<string, bool> _namespaceFilter;

        private ITupleSerializerInitializerProxy _jsConfigManager;

        internal ITupleSerializerInitializerProxy JsConfigProxy
        {
            get { return _jsConfigManager ?? (_jsConfigManager = new TupleSerializerInitializerProxy()); }
            set { _jsConfigManager = value; }
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

        public void Configure()
        {
            var publicTuples = _assembliesToScan.GetPublicTuples(_namespaceFilter);
            foreach (var publicTuple in publicTuples)
            {
                JsConfigProxy.ConfigInlineTupleSerializer(publicTuple);
            }
        }
    }
}
