using System;
using Xunit;

namespace DummyTests.Silverlight4.XUnit
{
    public class TestFixture1
    {
        [Fact]
        public void PassingTest1()
        {
        }

        [Fact]
        public void FailingTest1()
        {
            Assert.True(false);
        }

        [Fact(Skip="Ignore this test")]
        public void IgnoredTest1()
        {
        }

        [Fact]
        public void FailingTest4()
        {
            throw new InvalidOperationException();
        }
    }
}