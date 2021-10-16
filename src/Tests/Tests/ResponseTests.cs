using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phlank.Responder.Tests
{
    [TestClass]
    public class ResponseTests
    {
        [TestMethod]
        public void TestResponseProperties()
        {
            var responseT = new Response<string>();
            Assert.IsTrue(responseT.IsSuccessful);
            responseT.Problem = new Problem(400);
            Assert.IsFalse(responseT.IsSuccessful);
        }
    }
}
