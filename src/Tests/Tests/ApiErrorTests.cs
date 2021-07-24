using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Phlank.ApiModeling.Tests
{
    [TestClass]
    public class ApiErrorTests
    {
        [TestMethod]
        public void TestExceptionOnInitStatusCodeOutOfRange()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                var error = new ApiError
                {
                    Status = HttpStatusCode.OK,
                };
            });
        }
    }
}
