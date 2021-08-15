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
            return new ApiError
            {
                Detail = problemDetails.Detail,
                Extensions = new Dictionary<string, object>(problemDetails.Extensions),
                Instance = new Uri(problemDetails.Instance),
                Status = (HttpStatusCode)problemDetails.Status.Value,
                Title = problemDetails.Title,
                Type = new Uri(problemDetails.Type)
            };
        }
    }
}
