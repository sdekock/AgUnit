using System.Windows;
using Xunit;

namespace DummyTests.Silverlight4.XUnit
{
    public class Silverlight4RuntimeVersionTests
    {
        [Fact]
        public void DeploymentRuntimeVersionIsCorrect()
        {
            var systemAssembly = typeof(string).Assembly.FullName;
            Assert.True(systemAssembly.Contains("2.0.5.0"), string.Format("System assembly version was {0}, expected 2.0.5.0", systemAssembly));

            var runtimeVersion = Deployment.Current.RuntimeVersion;
            Assert.True(runtimeVersion.StartsWith("4."), string.Format("Runtime version was {0}, expected 4.*", runtimeVersion));
        }
    }
}