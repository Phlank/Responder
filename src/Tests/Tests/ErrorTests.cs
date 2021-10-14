using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace Phlank.Responder.Tests
{
    [TestClass]
    public class ErrorTests
    {
        [TestMethod]
        public void TestExceptionOnInitStatusCodeOutOfRange()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new ApiError(HttpStatusCode.OK);
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new ApiError(200);
            });
        }
    }
}
