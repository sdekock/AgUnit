using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DummyTests.Silverlight3
{
    [TestClass]
    public class Silverlight3RuntimeVersionTests
    {
        [TestMethod]
        public void DeploymentRuntimeVersionIsCorrect()
        {
            var enviromentVersion = Environment.Version;
            Assert.AreEqual(enviromentVersion, new Version(2, 0, 5, 0), string.Format("Environment version was {0}, expected 2.0.5.0", enviromentVersion));

            var runtimeVersion = Deployment.Current.RuntimeVersion;
            Assert.IsTrue(runtimeVersion.StartsWith("3."), string.Format("Runtime version was {0}, expected 3.*", runtimeVersion));
        }
    }
}