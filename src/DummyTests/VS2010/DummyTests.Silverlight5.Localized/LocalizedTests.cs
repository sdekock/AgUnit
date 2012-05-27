using System.Globalization;
using System.Threading;
using DummyTests.Silverlight5.Library.Localized;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DummyTests.Silverlight5.Localized
{
    [TestClass]
    public class LocalizedTests
    {
        [TestMethod]
        public void LocalizedTest()
        {
            var currentUICulture = Thread.CurrentThread.CurrentUICulture;

            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
                Assert.AreEqual("en-GB", TestResources.Language);
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = currentUICulture;
            }
        }
    }
}