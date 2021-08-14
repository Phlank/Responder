using Microsoft.AspNetCore.Mvc;

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
    public class ApiResult : JsonResult
    {
        internal ApiResult() : base(null) { }

        internal ApiResult(object response) : base(response) { }
    }
}
