using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phlank.ApiModeling.Tests.Helpers
{
    [ApiController]
    public class TestController
    {
        private readonly IApiResultBuilder _resultBuilder;

        public TestController(IApiResultBuilder resultBuilder) 
        {
            _resultBuilder = resultBuilder;
        }

        [Route("api/TestMethod")]
        public ApiResult TestMethod(TestModel model)
        {
            return _resultBuilder.Build();
        }
    }
}
