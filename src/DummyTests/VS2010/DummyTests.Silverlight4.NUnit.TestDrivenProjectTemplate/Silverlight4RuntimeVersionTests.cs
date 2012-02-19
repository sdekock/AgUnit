using System.Windows;
using NUnit.Framework;

namespace DummyTests.Silverlight4.NUnit.TestDrivenProjectTemplate
{
    [TestFixture]
    public class Silverlight4RuntimeVersionTests
    {
        [Test]
        public void DeploymentRuntimeVersionIsCorrect()
        {
            var runtimeVersion = Deployment.Current.RuntimeVersion;
            Assert.IsTrue(runtimeVersion.StartsWith("4."), string.Format("Runtime version was {0}, expected 4.*", runtimeVersion));
        }

        [Test]
        public void SystemAssemblyVersionIsCorrect()
        {
            var systemAssembly = typeof(string).Assembly.FullName;
            Assert.IsTrue(systemAssembly.Contains("2.0.5.0"), string.Format("System assembly version was {0}, expected 2.0.5.0", systemAssembly));
        }
    }
}