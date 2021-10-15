using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Phlank.Responder.Tests
{
    [TestClass]
    public class ApiErrorTests
    {
        [TestMethod]
        public void TestConstructor()
        {
            var errorWithAllValuesDefined = new ApiError(
                HttpStatusCode.BadRequest,
                "TestTitle",
                "TestDetail",
                new Uri("https://localhost/testtype"),
                new Uri("https://localhost/testinstance"),
                new Dictionary<string, object>()
                {
                    { "testExtension", "TestExtensionValue" }
                });
            Assert.AreEqual(HttpStatusCode.BadRequest, errorWithAllValuesDefined.Status);
            Assert.AreEqual("TestTitle", errorWithAllValuesDefined.Title);
            Assert.AreEqual("TestDetail", errorWithAllValuesDefined.Detail);
            Assert.AreEqual("https://localhost/testtype", errorWithAllValuesDefined.Type.OriginalString);
            Assert.AreEqual("https://localhost/testinstance", errorWithAllValuesDefined.Instance.OriginalString);
            Assert.AreEqual("TestExtensionValue", errorWithAllValuesDefined.Extensions["testExtension"]);

            var errorWithOnlyStatusDefined = new ApiError(HttpStatusCode.BadRequest);
            Assert.AreEqual(HttpStatusCode.BadRequest, errorWithOnlyStatusDefined.Status);
            Assert.AreEqual("BadRequest", errorWithOnlyStatusDefined.Title);
            Assert.AreEqual("The request could not be understood by the server.", errorWithOnlyStatusDefined.Detail);
            Assert.AreEqual("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1", errorWithOnlyStatusDefined.Type.OriginalString);
            Assert.IsNull(errorWithOnlyStatusDefined.Instance);
            Assert.IsTrue(errorWithOnlyStatusDefined.Extensions.Count() == 0);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new ApiError(HttpStatusCode.OK);
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new ApiError(200);
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new ApiError(700);
            });
        }

        [TestMethod]
        public void TestProperties()
        {
            var error = new ApiError(HttpStatusCode.BadRequest);
            error.Type = null;
            error.Instance = null;
            error.Extensions = null;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { error.Status = HttpStatusCode.OK; });
            Assert.ThrowsException<ArgumentNullException>(() => { error.Title = null; });
            Assert.ThrowsException<ArgumentNullException>(() => { error.Detail = null; });
            Assert.AreEqual("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1", error.Type.OriginalString);
            Assert.IsNull(error.Instance);
            Assert.IsTrue(error.Extensions.Count() == 0);
        }
    }
}
