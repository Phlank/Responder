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
            var response = new Response();
            Assert.IsTrue(response.IsSuccessful);
            response.Error = new Problem(400);
            Assert.IsFalse(response.IsSuccessful);

            var responseT = new Response<string>();
            Assert.IsTrue(responseT.IsSuccessful);
            responseT.Error = new Problem(400);
            Assert.IsFalse(responseT.IsSuccessful);
        }
    }
}
