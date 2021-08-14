using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace Phlank.Responder.Tests
{
    [TestClass]
    public class ApiErrorTests
    {
        [TestMethod]
        public void TestExceptionOnInitStatusCodeOutOfRange()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var error = new ApiError
                {
                    Status = HttpStatusCode.OK,
                };
            });
        }
    }
}
