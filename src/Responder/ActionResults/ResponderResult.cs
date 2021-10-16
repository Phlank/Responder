using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;
using System.Linq;
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
    [ProducesErrorResponseType(typeof(Problem))]
    public class ResponderResult<T> : ISuccessResponse<T>, IConvertToActionResult where T : class
    {
        protected readonly ResponderOptions _options;
        protected readonly HttpStatusCode _successfulStatusCode;
        private readonly Response<T> _response;

        public IDictionary<string, object> Extensions => _response.Extensions;
        public T Data => _response.Data;
        public bool IsSuccessful => _response.IsSuccessful;

        internal ResponderResult(Response<T> response, HttpStatusCode successfulStatusCode)
        {
            _response = response;
            _successfulStatusCode = successfulStatusCode;
        }

        public IActionResult Convert()
        {
            if (_response.IsSuccessful)
            {
                if (_response.Data == null && (_response.Extensions == null || _response.Extensions.Count() == 0))
                {
                    return new StatusCodeResult((int)_successfulStatusCode);
                }
                else
                {
                    return new JsonResult(_response)
                    {
                        ContentType = "application/json",
                        StatusCode = (int)_successfulStatusCode,
                    };
                }
            }
            else
            {
                return new JsonResult(_response.Problem)
                {
                    ContentType = "application/problem+json",
                    StatusCode = (int)_response.Problem.Status
                };
            }
        }
    }
}
