using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;

namespace Phlank.Responder.Tests.Data
{
    public static class Mocks
    {
        public static IMock<Controller> Controller()
        {
            var mock = new Mock<MockableController>();
            mock.SetupGet(e => e.ControllerContext).Returns(ControllerContext().Object);
            return mock;
        }

        public static IMock<ControllerContext> ControllerContext()
        {
            var mock = new Mock<MockableControllerContext>();
            mock.SetupGet(e => e.HttpContext.TraceIdentifier).Returns("TestTrace");
            return mock;
        }
    }
}
