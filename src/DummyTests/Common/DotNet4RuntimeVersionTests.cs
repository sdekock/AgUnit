using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DummyTests
{
    [TestClass]
    public class DotNet4RuntimeVersionTests
    {
        [TestMethod]
        public void DeploymentRuntimeVersionIsCorrect()
        {
            var enviromentVersion = Environment.Version;
            Assert.AreEqual(enviromentVersion.Major, 4, string.Format("Environment version was {0}, expected 4.*", enviromentVersion));
        }
    }
}