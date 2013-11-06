using System;
using System.Reflection;
using ExampleTuples;
using Xunit;

namespace ServiceStack.Text.TupleSerializer.UnitTests
{
    public class TupleSerializerConfiguratorTests
    {
        [Fact]
        public void Configure_TestAssembly_ConfiguresAllTuples()
        {
            var proxyFake = new TupleSerializerInitializerProxyFake();

            new TupleSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithAssemblies(new[] { Assembly.GetExecutingAssembly() })
                .Configure();

            Assert.Equal(4, proxyFake.ConfigedTypes.Count);
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(ExampleTupleWithoutNamespace)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(Tuple<double, double>)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(ObjectThatInheritsFromTuple)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(Tuple<string, string, string>)));
        }

        [Fact]
        public void Configure_TestAssemblyFilteredByNamespace_ConfiguresTuplesInNamespace()
        {
            var proxyFake = new TupleSerializerInitializerProxyFake();

            new TupleSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithAssemblies(new[] { Assembly.GetExecutingAssembly() })
                .WithNamespaceFilter(s => s.Equals("ExampleTuples", StringComparison.OrdinalIgnoreCase))
                .Configure();

            Assert.Equal(2, proxyFake.ConfigedTypes.Count);
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(ObjectThatInheritsFromTuple)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(Tuple<double, double>)));
        }

        [Fact]
        public void Configure_TestAssemblyFilteredToGenericParametersNamespace_DoesNotConfigureAnyTuples()
        {
            var proxyFake = new TupleSerializerInitializerProxyFake();

            new TupleSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithAssemblies(new[] { Assembly.GetExecutingAssembly() })
                .WithNamespaceFilter(s => s.Equals("GenericParameters", StringComparison.OrdinalIgnoreCase))
                .Configure();

            Assert.Equal(0, proxyFake.ConfigedTypes.Count);
        }

        [Fact]
        public void Configure_TestAssemblyFilteredByEmptyNamespace_ConfiguresTuplesWithoutANamespace()
        {
            var proxyFake = new TupleSerializerInitializerProxyFake();

            new TupleSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithAssemblies(new[] { Assembly.GetExecutingAssembly() })
                .WithNamespaceFilter(s => s.Equals(string.Empty, StringComparison.OrdinalIgnoreCase))
                .Configure();

            Assert.Equal(1, proxyFake.ConfigedTypes.Count);
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(ExampleTupleWithoutNamespace)));
        }

        [Fact]
        public void Configure_TestAssembly_JsConfigFunctionsSet()
        {
            lock (StaticTestingLocks.JsConfigLockObject)
            {
                JsConfig<Tuple<string, string, string>>.Reset();

                new TupleSerializerConfigurator()
                    .WithAssemblies(new[] {Assembly.GetExecutingAssembly()})
                    .WithNamespaceFilter(s => s.Equals("SingularTupleExample", StringComparison.OrdinalIgnoreCase))
                    .Configure();

                Assert.Equal("GetStringValue", JsConfig<Tuple<string, string, string>>.SerializeFn.Method.Name);
                Assert.Equal("GetTupleFrom", JsConfig<Tuple<string, string, string>>.DeSerializeFn.Method.Name);
            }
        }

        [Fact]
        public void WithAssemblies_EmptyList_DoesNotSetAnyAssemblies()
        {
            var configurator = new TupleSerializerConfigurator();

            configurator.WithAssemblies(null);

            Assert.True(configurator._assembliesToScan.IsEmpty());
        }
    }
}
