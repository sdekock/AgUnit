using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DummyTests.Silverlight3
{
    [TestClass]
    public class Silverlight4RuntimeVersionTests
    {
        [TestMethod]
        public void DeploymentRuntimeVersionIsCorrect()
        {
            var runtimeVersion = Deployment.Current.RuntimeVersion;
            Assert.IsTrue(runtimeVersion.StartsWith("3."), string.Format("Runtime version was {0}, expected 3.*", runtimeVersion));
        }

        [TestMethod]
        public void SystemAssemblyVersionIsCorrect()
        {
            var systemAssembly = typeof(string).Assembly.FullName;
            Assert.IsTrue(systemAssembly.Contains("2.0.5.0"), string.Format("System assembly version was {0}, expected 2.0.5.0", systemAssembly));
        }
    }
}