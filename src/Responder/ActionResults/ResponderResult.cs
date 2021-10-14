using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Phlank.Responder
{
    /// <summary>
    /// An action result produced by the <see cref="IResponder"/>. This
    /// object should be returned by any client-facing controller methods in
    /// your project. This result can hold any one of three forms of content:
    /// <list type="bullet">
    ///     <item>
    ///         No content with successful status code.
    ///     </item>
    ///     <item>
    ///         JSON content (<c>application/json</c>) with a successful status
    ///         code.
    ///     </item>
    ///     <item>
    ///         JSON content (<c>application/problem+json</c>) with an
    ///         unsuccessful status cude.
    ///     </item>
    /// </list>
    /// Each of these items will contain at the very least an HTTP status code
    /// and a response type header.
    /// </summary>
    public class ResponderResult : ActionResult, IActionResult
    {
        private readonly ResponderOptions _options;
        private readonly HttpStatusCode _successfulStatusCode;

        [System.Text.Json.Serialization.JsonExtensionData]
        [Newtonsoft.Json.JsonExtensionData]
        public readonly Response Response;

        internal ResponderResult(Response response, HttpStatusCode successfulStatusCode)
        {
            Response = response;
            _successfulStatusCode = successfulStatusCode;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            if (Response.IsSuccessful)
            {
                if (Response.Data == null && Response.Warnings == null)
                {
                    return new StatusCodeResult((int)_successfulStatusCode).ExecuteResultAsync(context);
                }
                else
                {
                    return new JsonResult(Response)
                    {
                        ContentType = "application/json",
                        StatusCode = (int)_successfulStatusCode,
                    }.ExecuteResultAsync(context);
                }
            }
            else
            {
                return new JsonResult(Response)
                {
                    ContentType = "application/problem+json",
                    StatusCode = (int)Response.Error.Status
                }.ExecuteResultAsync(context);
            }
        }
    }

    /// <summary>
    /// An action result produced by the <see cref="IResponder"/>. This
    /// object should be returned by any client-facing controller methods in
    /// your project. This result can hold any one of three forms of content:
    /// <list type="bullet">
    ///     <item>
    ///         No content with successful status code.
    ///     </item>
    ///     <item>
    ///         JSON content (<c>application/json</c>) with a successful status
    ///         code.
    ///     </item>
    ///     <item>
    ///         JSON content (<c>application/problem+json</c>) with an
    ///         unsuccessful status cude.
    ///     </item>
    /// </list>
    /// Each of these items will contain at the very least an HTTP status code
    /// and a response type header.
    /// </summary>
    public class ResponderResult<T> : ActionResult, IActionResult where T : class
    {
        private readonly ResponderOptions _options;
        private readonly HttpStatusCode _successfulStatusCode;

        [System.Text.Json.Serialization.JsonExtensionData]
        [Newtonsoft.Json.JsonExtensionData]
        public readonly Response<T> Response;

        internal ResponderResult(Response<T> response, HttpStatusCode successfulStatusCode)
        {
            Response = response;
            _successfulStatusCode = successfulStatusCode;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            if (Response.IsSuccessful)
            {
                if (Response.Data == null && Response.Warnings == null)
                {
                    return new StatusCodeResult((int)_successfulStatusCode).ExecuteResultAsync(context);
                }
                else
                {
                    return new JsonResult(Response)
                    {
                        ContentType = "application/json",
                        StatusCode = (int)_successfulStatusCode,
                    }.ExecuteResultAsync(context);
                }
            }
            else
            {
                return new JsonResult(Response)
                {
                    ContentType = "application/problem+json",
                    StatusCode = (int)Response.Error.Status
                }.ExecuteResultAsync(context);
            }
        }
    }
}
