using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Phlank.Responder.Tests.Data;
using System.Linq;

namespace Phlank.Responder.Tests.Tests
{
    [TestClass]
    public class ResponderTests
    {
        private Controller _controller;
        private IResponder _responder;
        private Problem _badRequest;
        private Problem _unauthorized;

        [TestInitialize]
        public void Initialize()
        {
            _responder = new Responder();
            _controller = Data.Mocks.Controller().Object;
            _badRequest = Data.Samples.BadRequestProblem;
            _unauthorized = Data.Samples.UnauthorizedProblem;
        }

        [TestMethod]
        public void TestBasicBuild()
        {
            var responderResult = _responder.Build(_controller);
            Assert.IsNull(responderResult.Response.Error);
            Assert.IsNull(responderResult.Response.Data);
            Assert.IsTrue(responderResult.Response.Warnings.Count() == 0);

            var responderResultT = _responder.Build<string>(_controller);
            Assert.IsNull(responderResultT.Response.Error);
            Assert.IsNull(responderResultT.Response.Data);
            Assert.IsTrue(responderResultT.Response.Warnings.Count() == 0);
        }

        public void TestProblemBuild()
        {
            _responder.AddProblem(_badRequest);

            var responderResult = _responder.Build(_controller);
            Assert.AreEqual("TestProblem", responderResult.Response.Error.Title);
            Assert.AreEqual("TestDetail", responderResult.Response.Error.Detail);
            Assert.AreEqual("https://testtype", responderResult.Response.Error.Type.OriginalString);
            Assert.AreEqual("https://testinstance", responderResult.Response.Error.Instance.OriginalString);
            Assert.AreEqual("ExtensionValue", responderResult.Response.Error.Extensions["extensionName"]);
            Assert.AreEqual("TestTrace", responderResult.Response.Error.Extensions["traceId"]);
            Assert.IsNull(responderResult.Response.Data);
            Assert.IsNull(responderResult.Response.Warnings);

            var responderResultT = _responder.Build<string>(_controller);
            Assert.AreEqual("TestProblem", responderResultT.Response.Error.Title);
            Assert.AreEqual("TestDetail", responderResultT.Response.Error.Detail);
            Assert.AreEqual("https://testtype", responderResultT.Response.Error.Type.OriginalString);
            Assert.AreEqual("https://testinstance", responderResultT.Response.Error.Instance.OriginalString);
            Assert.AreEqual("ExtensionValue", responderResultT.Response.Error.Extensions["extensionName"]);
            Assert.AreEqual("TestTrace", responderResultT.Response.Error.Extensions["traceId"]);
            Assert.IsNull(responderResultT.Response.Data);
            Assert.IsNull(responderResultT.Response.Warnings);
        }
    }
}
