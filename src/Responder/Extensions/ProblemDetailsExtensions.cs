using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Phlank.Responder.Extensions
{
    internal static class ProblemDetailsExtensions
    {
        public static Problem ToProblem(this ProblemDetails problemDetails)
        {
            return new Problem(
                problemDetails.Status.Value,
                title: problemDetails.Title,
                detail: problemDetails.Detail,
                type: new Uri(problemDetails.Type),
                instance: new Uri(problemDetails.Instance));
        }
    }
}
