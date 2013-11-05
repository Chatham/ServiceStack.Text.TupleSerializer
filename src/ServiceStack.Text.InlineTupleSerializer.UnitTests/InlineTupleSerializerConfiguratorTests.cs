using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Text.InlineTupleSerializer.UnitTests.TestCases;

namespace ServiceStack.Text.InlineTupleSerializer.UnitTests
{
    [TestClass]
    public class InlineTupleSerializerConfiguratorTests
    {
        private const string TEST_CASE_NAMESPACE = "ServiceStack.Text.InlineTupleSerializer.UnitTests.TestCases";

        [TestMethod]
        public void Configure_TestAssembly_ConfiguresAllTuples()
        {
            var proxyFake = new TupleSerializerInitializerProxyFake();

            new InlineTupleSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithAssemblies(new[] { Assembly.GetExecutingAssembly() })
                .Configure();

            Assert.AreEqual(5, proxyFake.ConfigedTypes.Count);
            Assert.IsTrue(proxyFake.ConfigedTypes.Contains(typeof(ExampleTupleWithoutNamespace)));
            Assert.IsTrue(proxyFake.ConfigedTypes.Contains(typeof(StringPair)));
            Assert.IsTrue(proxyFake.ConfigedTypes.Contains(typeof(StringTriad)));
            Assert.IsTrue(proxyFake.ConfigedTypes.Contains(typeof(Tuple<string>)));
            Assert.IsTrue(proxyFake.ConfigedTypes.Contains(typeof(Tuple<string, string, string>)));
        }

        [TestMethod]
        public void Configure_TestAssemblyFilteredByNamespace_ConfiguresTuplesInNamespace()
        {
            var proxyFake = new TupleSerializerInitializerProxyFake();

            new InlineTupleSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithAssemblies(new[] { Assembly.GetExecutingAssembly() })
                .WithNamespaceFilter(s => s.Equals(TEST_CASE_NAMESPACE, StringComparison.OrdinalIgnoreCase))
                .Configure();

            Assert.AreEqual(4, proxyFake.ConfigedTypes.Count);
            Assert.IsTrue(proxyFake.ConfigedTypes.Contains(typeof(StringPair)));
            Assert.IsTrue(proxyFake.ConfigedTypes.Contains(typeof(StringTriad)));
            Assert.IsTrue(proxyFake.ConfigedTypes.Contains(typeof(Tuple<string>)));
            Assert.IsTrue(proxyFake.ConfigedTypes.Contains(typeof(Tuple<string, string, string>)));
        }

        [TestMethod]
        public void Configure_TestAssemblyFilteredByEmptyNamespace_ConfiguresTuplesWithoutANamespace()
        {
            var proxyFake = new TupleSerializerInitializerProxyFake();

            new InlineTupleSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithAssemblies(new[] { Assembly.GetExecutingAssembly() })
                .WithNamespaceFilter(s => s.Equals(string.Empty, StringComparison.OrdinalIgnoreCase))
                .Configure();

            Assert.AreEqual(1, proxyFake.ConfigedTypes.Count);
            Assert.IsTrue(proxyFake.ConfigedTypes.Contains(typeof(ExampleTupleWithoutNamespace)));
        }

        [TestMethod]
        public void Configure_TestAssembly_JsConfigFunctionsSet()
        {
            lock (StaticTestingLocks.JsConfigLockObject)
            {
                JsConfig<Tuple<string, string, string>>.Reset();

                new InlineTupleSerializerConfigurator()
                    .WithAssemblies(new[] {Assembly.GetExecutingAssembly()})
                    .WithNamespaceFilter(s => s.Equals("Tupletastic", StringComparison.OrdinalIgnoreCase))
                    .Configure();

                Assert.AreEqual("GetStringValue", JsConfig<Tuple<string, string, string>>.SerializeFn.Method.Name);
                Assert.AreEqual("GetTupleFrom", JsConfig<Tuple<string, string, string>>.DeSerializeFn.Method.Name);
            }
        }

        [TestMethod]
        public void WithAssemblies_EmptyList_DoesNotSetAnyAssemblies()
        {
            var configurator = new InlineTupleSerializerConfigurator();

            configurator.WithAssemblies(null);

            Assert.IsTrue(configurator._assembliesToScan.IsEmpty());
        }
    }
}
