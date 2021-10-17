using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Phlank.Responder.ActionResults;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    public class ResponderResult : ISuccessResponse, IProblemResponse, IConvertToActionResult
    {
        protected readonly ResponderOptions _options;
        protected readonly HttpStatusCode _successfulStatusCode;
        protected readonly Response _response;

        public ResponderResult(Response response, HttpStatusCode successfulStatusCode)
        {
            _response = response;
            _successfulStatusCode = successfulStatusCode;
        }

        [System.Text.Json.Serialization.JsonExtensionData]
        [Newtonsoft.Json.JsonExtensionData]
        public IDictionary<string, object> Extensions => _response.Extensions;
        public bool IsSuccessful => Problem == null;

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Problem Problem => _response.Problem;

        public virtual IActionResult Convert()
        {
            return ResultConverter.Convert(this, _successfulStatusCode);
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
    public class ResponderResult<T> : ResponderResult, ISuccessResponse<T>, IConvertToActionResult where T : class
    {
        protected readonly new Response<T> _response;

        public T Data => _response.Data;

        internal ResponderResult(Response<T> response, HttpStatusCode successfulStatusCode) : base(response, successfulStatusCode) 
        {
            _response = response;
        }

        public override IActionResult Convert()
        {
            return ResultConverter.Convert(this, _successfulStatusCode);
        }
    }
}
