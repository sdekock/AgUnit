using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DummyTests.Silverlight5.Library.Localized
{
    [TestClass]
    public class Silverlight5RuntimeVersionTests
    {
        [TestMethod]
        public void DeploymentRuntimeVersionIsCorrect()
        {
            var runtimeVersion = Deployment.Current.RuntimeVersion;
            Assert.IsTrue(runtimeVersion.StartsWith("5."), string.Format("Runtime version was {0}, expected 5.*", runtimeVersion));
        }

        [TestMethod]
        public void SystemAssemblyVersionIsCorrect()
        {
            var systemAssembly = typeof(string).Assembly.FullName;
            Assert.IsTrue(systemAssembly.Contains("5.0.5.0"), string.Format("System assembly version was {0}, expected 5.0.5.0", systemAssembly));
        }
    }
}