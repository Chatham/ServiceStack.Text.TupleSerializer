using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceStack.Text.InlineTupleSerializer.UnitTests
{
    [TestClass]
    public class AssemblyExtensionsTests
    {
        private const string TEST_CASE_NAMESPACE = "ServiceStack.Text.InlineTupleSerializer.UnitTests.TestCases";

        private List<Assembly> GetAssembliesInTestFolder()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var allAssemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFile).ToList();

            Console.WriteLine("Discovered the following assemblies:");
            foreach (var assembly in allAssemblies)
            {
                Console.WriteLine("  - " + assembly.FullName);
            }

            return allAssemblies;
        }

        [TestMethod]
        public void GetPublicTuples_TestFolderAssembliesFilteredByTestCaseNamespace_ReturnsFourTypes()
        {
            var testFolderAssemblies = GetAssembliesInTestFolder();
            var types = testFolderAssemblies.GetPublicTuples(s => s.Equals(TEST_CASE_NAMESPACE, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(4, types.Count);
        }

        [TestMethod]
        public void AlwaysTrueFilter_WhenInvoked_ReturnsTrue()
        {
            Assert.IsTrue(AssemblyExtensions.AlwaysTrueFilter(string.Empty));
        }
    }
}
