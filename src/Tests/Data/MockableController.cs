using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phlank.Responder.Tests.Data
{
    public class MockableController : Controller
    {
        public virtual new ControllerContext ControllerContext { get; set; }
    }

    public class MockableControllerContext : ControllerContext
    {
        public virtual new HttpContext HttpContext { get; set; }
    }
}
