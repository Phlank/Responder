using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phlank.Responder.Tests.Helpers
{
    [ApiController]
    public class TestController : Controller
    {
        private readonly IResponder _responder;

        public TestController(IResponder responder) 
        {
            _responder = responder;
        }

        [Route("api/TestMethod")]
        public ResponderResult TestMethod(TestModel model)
        {
            return _responder.Build(this);
        }
    }
}
