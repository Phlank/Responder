using Microsoft.AspNetCore.Http;
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
        private HttpContext _context;
        private IResponder _responder;
        private Problem _badRequest;
        private Problem _unauthorized;

        [TestInitialize]
        public void Initialize()
        {
            _responder = new Responder();
            _context = Data.Mocks.HttpContext().Object;
            _badRequest = Data.Samples.BadRequestProblem;
            _unauthorized = Data.Samples.UnauthorizedProblem;
        }

        [TestMethod]
        public void TestBasicBuild()
        {
            var responderResultT = _responder.Build<string>(_context);
            Assert.IsTrue(responderResultT.Extensions.Count() == 0);
            Assert.IsNull(responderResultT.Data);
            Assert.IsTrue(responderResultT.IsSuccessful);
        }

        [TestMethod]
        public void TestProblemBuild()
        {
            _responder.AddProblem(_badRequest);

            var responderResult = _responder.Build<string>(_context);
            Assert.AreEqual("BadRequest", responderResult.Problem.Title);
            Assert.AreEqual("The request could not be understood by the server.", responderResult.Problem.Detail);
            Assert.AreEqual("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1", responderResult.Problem.Type.OriginalString);
            Assert.IsNull(responderResult.Problem.Instance);
            Assert.AreEqual("TestTrace", responderResult.Problem.Extensions["traceId"]);
            Assert.IsNull(responderResult.Extensions);
            Assert.IsNull(responderResult.Data);
            Assert.IsFalse(responderResult.IsSuccessful);
        }
    }
}
