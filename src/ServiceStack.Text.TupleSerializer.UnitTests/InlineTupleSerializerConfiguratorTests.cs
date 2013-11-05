using System;
using System.Reflection;
using ExampleTuples;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceStack.Text.TupleSerializer.UnitTests
{
    [TestClass]
    public class InlineTupleSerializerConfiguratorTests
    {
        [TestMethod]
        public void Configure_TestAssembly_ConfiguresAllTuples()
        {
            var proxyFake = new TupleSerializerInitializerProxyFake();

            new TupleSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithAssemblies(new[] { Assembly.GetExecutingAssembly() })
                .Configure();

            Assert.AreEqual(4, proxyFake.ConfigedTypes.Count);
            Assert.IsTrue(proxyFake.ConfigedTypes.Contains(typeof(ExampleTupleWithoutNamespace)));
            Assert.IsTrue(proxyFake.ConfigedTypes.Contains(typeof(Tuple<double, double>)));
            Assert.IsTrue(proxyFake.ConfigedTypes.Contains(typeof(ObjectThatInheritsFromTuple)));
            Assert.IsTrue(proxyFake.ConfigedTypes.Contains(typeof(Tuple<string, string, string>)));
        }

        [TestMethod]
        public void Configure_TestAssemblyFilteredByNamespace_ConfiguresTuplesInNamespace()
        {
            var proxyFake = new TupleSerializerInitializerProxyFake();

            new TupleSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithAssemblies(new[] { Assembly.GetExecutingAssembly() })
                .WithNamespaceFilter(s => s.Equals("ExampleTuples", StringComparison.OrdinalIgnoreCase))
                .Configure();

            Assert.AreEqual(2, proxyFake.ConfigedTypes.Count);
            Assert.IsTrue(proxyFake.ConfigedTypes.Contains(typeof(ObjectThatInheritsFromTuple)));
            Assert.IsTrue(proxyFake.ConfigedTypes.Contains(typeof(Tuple<double, double>)));
        }

        [TestMethod]
        public void Configure_TestAssemblyFilteredToGenericParametersNamespace_DoesNotConfigureAnyTuples()
        {
            var proxyFake = new TupleSerializerInitializerProxyFake();

            new TupleSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithAssemblies(new[] { Assembly.GetExecutingAssembly() })
                .WithNamespaceFilter(s => s.Equals("GenericParameters", StringComparison.OrdinalIgnoreCase))
                .Configure();

            Assert.AreEqual(0, proxyFake.ConfigedTypes.Count);
        }

        [TestMethod]
        public void Configure_TestAssemblyFilteredByEmptyNamespace_ConfiguresTuplesWithoutANamespace()
        {
            var proxyFake = new TupleSerializerInitializerProxyFake();

            new TupleSerializerConfigurator { JsConfigProxy = proxyFake }
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

                new TupleSerializerConfigurator()
                    .WithAssemblies(new[] {Assembly.GetExecutingAssembly()})
                    .WithNamespaceFilter(s => s.Equals("SingularTupleExample", StringComparison.OrdinalIgnoreCase))
                    .Configure();

                Assert.AreEqual("GetStringValue", JsConfig<Tuple<string, string, string>>.SerializeFn.Method.Name);
                Assert.AreEqual("GetTupleFrom", JsConfig<Tuple<string, string, string>>.DeSerializeFn.Method.Name);
            }
        }

        [TestMethod]
        public void WithAssemblies_EmptyList_DoesNotSetAnyAssemblies()
        {
            var configurator = new TupleSerializerConfigurator();

            configurator.WithAssemblies(null);

            Assert.IsTrue(configurator._assembliesToScan.IsEmpty());
        }
    }
}
