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
            var problemWithAllValuesDefined = new Problem(
                HttpStatusCode.BadRequest,
                "TestTitle",
                "TestDetail",
                new Uri("https://localhost/testtype"),
                new Uri("https://localhost/testinstance"),
                new Dictionary<string, object>()
                {
                    { "testExtension", "TestExtensionValue" }
                });
            Assert.AreEqual(HttpStatusCode.BadRequest, problemWithAllValuesDefined.Status);
            Assert.AreEqual("TestTitle", problemWithAllValuesDefined.Title);
            Assert.AreEqual("TestDetail", problemWithAllValuesDefined.Detail);
            Assert.AreEqual("https://localhost/testtype", problemWithAllValuesDefined.Type.OriginalString);
            Assert.AreEqual("https://localhost/testinstance", problemWithAllValuesDefined.Instance.OriginalString);
            Assert.AreEqual("TestExtensionValue", problemWithAllValuesDefined.Extensions["testExtension"]);

            var problemWithOnlyStatusDefined = new Problem(HttpStatusCode.BadRequest);
            Assert.AreEqual(HttpStatusCode.BadRequest, problemWithOnlyStatusDefined.Status);
            Assert.AreEqual("BadRequest", problemWithOnlyStatusDefined.Title);
            Assert.AreEqual("The request could not be understood by the server.", problemWithOnlyStatusDefined.Detail);
            Assert.AreEqual("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1", problemWithOnlyStatusDefined.Type.OriginalString);
            Assert.IsNull(problemWithOnlyStatusDefined.Instance);
            Assert.IsTrue(problemWithOnlyStatusDefined.Extensions.Count() == 0);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new Problem(HttpStatusCode.OK);
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new Problem(200);
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                new Problem(700);
            });
        }

        [TestMethod]
        public void TestProperties()
        {
            var error = new Problem(HttpStatusCode.BadRequest);
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
