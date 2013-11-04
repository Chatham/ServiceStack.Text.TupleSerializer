using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ServiceStack.Text.InlineTupleSerializer.Api;

namespace ServiceStack.Text.InlineTupleSerializer
{
    public sealed class InlineTupleSerializerConfigurator : IInlineTupleSerializerConfigurator
    {
        private readonly HashSet<Assembly> _assembliesToScan = new HashSet<Assembly>();

        private Func<string, bool> _namespaceFilter;

        private ITupleSerializerInitializerProxy _jsConfigManager;

        internal ITupleSerializerInitializerProxy JsConfigProxy
        {
            get { return _jsConfigManager ?? (_jsConfigManager = new TupleSerializerInitializerProxy()); }
            set { _jsConfigManager = value; }
        }

        public IInlineTupleSerializerConfigurator WithNamespaceFilter(Func<string, bool> namespaceFilter)
        {
            _namespaceFilter = namespaceFilter;
            return this;
        }

        public IInlineTupleSerializerConfigurator WithAssemblies(ICollection<Assembly> assembliesToScan)
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
            Parallel.ForEach(publicTuples, JsConfigProxy.ConfigInlineTupleSerializer);
        }
    }
}
