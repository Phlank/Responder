using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phlank.Responder.Tests
{
    [TestClass]
    public class WarningTests
    {
        [TestMethod]
        public void TestWarningConstructor()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Warning(Severity.High, null));

            var warningWithNoExtensions = new Warning(Severity.High, "TestMessage");
            Assert.IsTrue(warningWithNoExtensions.Extensions.Count() == 0);

            var warningWithExtensions = new Warning(Severity.High, "TestMessage", new Dictionary<string, object> { { "testExtension", "ExtensionValue" } });
            Assert.AreEqual("ExtensionValue", warningWithExtensions.Extensions["testExtension"]);
        }

        [TestMethod]
        public void TestWarningProperties()
        {
            var warning = new Warning(Severity.High, "TestMessage");
            Assert.ThrowsException<ArgumentNullException>(() => { warning.Message = null; });
            warning.Extensions = null;
            Assert.IsTrue(warning.Extensions.Count() == 0);
        }
    }
}
