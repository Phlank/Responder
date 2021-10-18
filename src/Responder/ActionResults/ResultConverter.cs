using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace Phlank.Responder.ActionResults
{
    internal static class ResultConverter
    {
        public static IActionResult Convert(ResponderResult result, HttpStatusCode successStatus)
        {
            if (result.IsSuccessful)
            {
                if (result.Extensions == null || result.Extensions.Count() == 0)
                {
                    return new StatusCodeResult((int)successStatus);
                }
                else
                {
                    return new JsonResult(result.Extensions)
                    {
                        ContentType = "application/json",
                        StatusCode = (int)successStatus,
                    };
                }
            }
            else
            {
                return new JsonResult(result.Problem)
                {
                    ContentType = "application/problem+json",
                    StatusCode = (int)result.Problem.Status
                };
            }
        }

        public static IActionResult Convert<T>(ResponderResult<T> result, HttpStatusCode successStatus) where T : class
        {
            if (result.IsSuccessful)
            {
                if (result.Extensions == null || result.Extensions.Count() == 0)
                {
                    return new StatusCodeResult((int)successStatus);
                }
                else
                {
                    return new JsonResult(new { result.Data, result.Extensions })
                    {
                        ContentType = "application/json",
                        StatusCode = (int)successStatus,
                    };
                }
            }
            else
            {
                return new JsonResult(result.Problem)
                {
                    ContentType = "application/problem+json",
                    StatusCode = (int)result.Problem.Status
                };
            }
        }
    }
}
