using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Phlank.Responder.Tests.Data
{
    public static class Samples
    {
        public static Problem BadRequestProblem = new Problem(HttpStatusCode.BadRequest);

        public static Problem UnauthorizedProblem = new Problem(HttpStatusCode.Unauthorized);
    }
}
