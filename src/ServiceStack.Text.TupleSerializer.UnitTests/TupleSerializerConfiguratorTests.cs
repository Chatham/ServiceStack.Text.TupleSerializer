using System;
using System.Collections.Generic;
using System.Reflection;
using TupleWithNamespace;
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

            Assert.Equal(9, proxyFake.ConfigedTypes.Count);
        }

        [Fact]
        public void Configure_TestAssemblyFilteredByNamespace_ConfiguresTuplesInNamespace()
        {
            var proxyFake = new TupleSerializerInitializerProxyFake();

            new TupleSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithAssemblies(new[] { Assembly.GetExecutingAssembly() })
                .WithNamespaceFilter(s => s.Equals("TupleWithNamespace", StringComparison.OrdinalIgnoreCase))
                .Configure();

            Assert.Equal(3, proxyFake.ConfigedTypes.Count);
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(ObjectThatInheritsFromTuple)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(Tuple<string, string>)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(Tuple<double, double>)));
        }

        [Fact]
        public void Configure_TestAssemblyFilteredToGenericParametersNamespace_DoesNotConfigureAnyTuples()
        {
            var proxyFake = new TupleSerializerInitializerProxyFake();

            new TupleSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithAssemblies(new[] { Assembly.GetExecutingAssembly() })
                .WithNamespaceFilter(s => s.Equals("NestedInGenerics", StringComparison.OrdinalIgnoreCase))
                .Configure();

            Assert.Equal(4, proxyFake.ConfigedTypes.Count);
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(Tuple<string>)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(Tuple<int>)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(Tuple<bool, bool>)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(Tuple<int, int>)));
        }

        [Fact]
        public void Configure_TestAssemblyFilteredByEmptyNamespace_ConfiguresTuplesWithoutANamespace()
        {
            var proxyFake = new TupleSerializerInitializerProxyFake();

            new TupleSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithAssemblies(new[] { Assembly.GetExecutingAssembly() })
                .WithNamespaceFilter(s => s.Equals(string.Empty, StringComparison.OrdinalIgnoreCase))
                .Configure();

            Assert.Equal(2, proxyFake.ConfigedTypes.Count);
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(TupleWithoutNamespace)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(Tuple<int, string>)));
        }

        [Fact]
        public void Configure_SingularTupleType_JsConfigFunctionsSet()
        {
            lock (StaticTestingLocks.JsConfigLockObject)
            {
                JsConfig<Tuple<string, string, string>>.Reset();

                new TupleSerializerConfigurator()
                    .WithTupleTypes(new List<Type>{typeof(Tuple<string, string, string>)})
                    .Configure();

                Assert.Equal("GetStringValue", JsConfig<Tuple<string, string, string>>.SerializeFn.Method.Name);
                Assert.Equal("GetTupleFrom", JsConfig<Tuple<string, string, string>>.DeSerializeFn.Method.Name);
            }
        }

        [Fact]
        public void WithDelimiter_CustomDelimiter_SetsCustomDelimiter()
        {
            var configurator = new TupleSerializerConfigurator();

            configurator.WithDelimiter(":");

            Assert.Equal(":", configurator._delimiter);
        }

        [Fact]
        public void WithAssemblies_EmptyList_DoesNotSetAnyAssemblies()
        {
            var configurator = new TupleSerializerConfigurator();

            configurator.WithAssemblies(null);

            Assert.True(configurator._assembliesToScan.IsEmpty());
        }

        [Fact]
        public void WithTupleTypes_EmptyList_DoesNotRegisterAnyTuples()
        {
            var configurator = new TupleSerializerConfigurator();

            configurator.WithTupleTypes(null);

            Assert.True(configurator._tupleTypes.IsEmpty());
        }

        [Fact]
        public void WithTupleTypes_ListWithTupleTypesAndNonTupleTypes_OnlyRegistersTuples()
        {
            var configurator = new TupleSerializerConfigurator();

            configurator.WithTupleTypes(new List<Type>
            {
                typeof(Tuple<string,string>), typeof(NotSupportedException)
            });

            Assert.Equal(1, configurator._tupleTypes.Count);
            Assert.True(configurator._tupleTypes.Contains(typeof(Tuple<string, string>)));
        }

        [Fact]
        public void WithTupleTypes_ListWithNonRegisteredTupleType_AdditivelyRegistersTupleType()
        {
            var configurator = new TupleSerializerConfigurator();
            configurator._tupleTypes.Add(typeof(Tuple<bool, bool>));

            configurator.WithTupleTypes(new List<Type>
            {
                typeof(Tuple<string,string>)
            });

            Assert.Equal(2, configurator._tupleTypes.Count);
            Assert.True(configurator._tupleTypes.Contains(typeof(Tuple<bool, bool>)));
        }

        [Fact]
        public void WithTupleTypes_ListWithAlreadyRegisteredTupleType_DoesNotReRegisterTupleType()
        {
            var configurator = new TupleSerializerConfigurator();
            configurator._tupleTypes.Add(typeof(Tuple<bool, bool>));

            configurator.WithTupleTypes(new List<Type>
            {
                typeof(Tuple<bool,bool>)
            });

            Assert.Equal(1, configurator._tupleTypes.Count);
            Assert.True(configurator._tupleTypes.Contains(typeof(Tuple<bool, bool>)));
        }
    }
}
