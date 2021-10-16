using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using Phlank.Responder.ActionResults;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Phlank.Responder
{
    public class ResponderResult : ISuccessResponse, IConvertToActionResult
    {
        protected readonly ResponderOptions _options;
        protected readonly HttpStatusCode _successfulStatusCode;
        private readonly Response _response;

        public ResponderResult(Response response, HttpStatusCode successfulStatusCode)
        {
            _response = response;
            _successfulStatusCode = successfulStatusCode;
        }

        [System.Text.Json.Serialization.JsonExtensionData]
        [Newtonsoft.Json.JsonExtensionData]
        public IDictionary<string, object> Extensions => throw new System.NotImplementedException();
        public bool IsSuccessful => throw new System.NotImplementedException();

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Problem Problem => _response.Problem;

        public IActionResult Convert()
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
    public class ResponderResult<T> : ISuccessResponse<T>, IConvertToActionResult where T : class
    {
        protected readonly ResponderOptions _options;
        protected readonly HttpStatusCode _successfulStatusCode;
        private readonly Response<T> _response;

        [System.Text.Json.Serialization.JsonExtensionData]
        [Newtonsoft.Json.JsonExtensionData]
        public IDictionary<string, object> Extensions => _response.Extensions;
        public T Data => _response.Data;
        public bool IsSuccessful => _response.IsSuccessful;

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Problem Problem => _response.Problem;

        internal ResponderResult(Response<T> response, HttpStatusCode successfulStatusCode)
        {
            _response = response;
            _successfulStatusCode = successfulStatusCode;
        }

        public IActionResult ConvertAsync()
        {
            return ResultConverter.Convert(this, _successfulStatusCode);
        }
    }
}
