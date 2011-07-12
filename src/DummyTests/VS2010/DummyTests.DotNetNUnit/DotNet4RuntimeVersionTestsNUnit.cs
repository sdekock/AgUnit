using System;
using NUnit.Framework;

namespace DummyTests.DotNetNUnit
{
    [TestFixture]
    public class DotNet4RuntimeVersionTests
    {
        [Test]
        public void DeploymentRuntimeVersionIsCorrect()
        {
            var enviromentVersion = Environment.Version;
            Assert.AreEqual(enviromentVersion.Major, 4, string.Format("Environment version was {0}, expected 4.*", enviromentVersion));
        }
    }
}