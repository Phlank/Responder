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
        public static ApiError ToApiError(this ProblemDetails problemDetails)
        {
            return new ApiError(
                (HttpStatusCode)problemDetails.Status,
                title: problemDetails.Title,
                detail: problemDetails.Detail,
                type: new Uri(problemDetails.Type),
                instance: new Uri(problemDetails.Instance));
        }
    }
}
