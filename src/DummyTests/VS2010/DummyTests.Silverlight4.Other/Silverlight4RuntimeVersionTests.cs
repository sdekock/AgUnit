using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DummyTests.Silverlight4.Other
{
    [TestClass]
    public class Silverlight4RuntimeVersionTests
    {
        [TestMethod]
        public void DeploymentRuntimeVersionIsCorrect()
        {
            var enviromentVersion = Environment.Version;
            Assert.AreEqual(enviromentVersion, new Version(2, 0, 5, 0), string.Format("Environment version was {0}, expected 2.0.5.0", enviromentVersion));

            var runtimeVersion = Deployment.Current.RuntimeVersion;
            Assert.IsTrue(runtimeVersion.StartsWith("4."), string.Format("Runtime version was {0}, expected 4.*", runtimeVersion));
        }
    }
}