using System;
using NUnit.Framework;

namespace DummyTests.Silverlight4.NUnit
{
    [TestFixture]
    public class TestFixture1
    {
        [TestFixtureSetUp]
        public static void ClassInitialize1()
        {
        }

        [SetUp]
        public void TestInitialize1()
        {
        }

        [Test]
        public void PassingTest1()
        {
        }

        [Test]
        public void FailingTest1()
        {
            Assert.Fail();
        }

        [Ignore]
        [Test]
        public void IgnoredTest1()
        {
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PassingTest2()
        {
            throw new InvalidOperationException();
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void FailingTest2()
        {
            throw new InvalidOperationException();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FailingTest3()
        {
        }

        [Test]
        public void FailingTest4()
        {
            throw new InvalidOperationException();
        }

        [TearDown]
        public void TestCleanup1()
        {
        }

        [TestFixtureTearDown]
        public static void ClassCleanup1()
        {
        }
    }
}