using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DummyTests.Silverlight5
{
    [TestClass]
    public class ControlsTest
    {
        [TestMethod]
        public void TryInstantiateControl()
        {
            var button = new Button();
            Assert.IsNotNull(button);
        }
    }
}