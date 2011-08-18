using System;
using System.Reflection;
using NUnit.Framework;

namespace DummyTests.Silverlight4.NUnit
{
    [TestFixture]
    public class SilverlightTests
    {
        [Test]
        public void SystemVersion()
        {
            Assembly systemAssembly = typeof(Uri).Assembly;
            StringAssert.Contains("=2.0.5.0,", systemAssembly.FullName,
                "Check we're testing a Silverlight 2.0 assembly");
        }

        [Test]
        public void SystemVersion2()
        {
            Assembly systemAssembly = typeof(Uri).Assembly;
            //StringAssert.Contains("=2.0.5.0,", systemAssembly.FullName,
            //    "Check we're testing a Silverlight 2.0 assembly");

            Assert.IsTrue(true);

        }
    }
}
