using System;
using Xunit;

namespace DummyTests.DotNetXUnit
{
    public class DotNet4RuntimeVersionTests
    {
        [Fact]
        public void DeploymentRuntimeVersionIsCorrect()
        {
            var enviromentVersion = Environment.Version;
            Assert.Equal(enviromentVersion.Major, 4);
        }
    }
}