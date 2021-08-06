using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace Phlank.ApiModeling.Tests
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
