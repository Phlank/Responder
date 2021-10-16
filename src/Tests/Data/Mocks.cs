using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;

namespace Phlank.Responder.Tests.Data
{
    public static class Mocks
    {
        public static IMock<ControllerBase> Controller()
        {
            var mock = new Mock<MockableController>();
            var controllerContext = ControllerContext().Object;
            var httpContext = HttpContext().Object;
            mock.Setup(e => e.ControllerContext).Returns(controllerContext);
            mock.Setup(e => e.HttpContext).Returns(httpContext);
            return mock;
        }

        public static IMock<ControllerContext> ControllerContext()
        {
            var mock = new Mock<MockableControllerContext>();
            var httpContext = HttpContext().Object;
            mock.Setup(e => e.HttpContext).Returns(httpContext);
            return mock;
        }

        public static IMock<HttpContext> HttpContext()
        {
            var mock = new Mock<HttpContext>();
            mock.Setup(e => e.TraceIdentifier).Returns("TestTrace");
            return mock;
        }
    }
}
