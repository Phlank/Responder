using Newtonsoft.Json;
using Phlank.Responder.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace Phlank.Responder
{
    /// <summary>
    /// An error to be reflected by the API. This class follows the
    /// specification in
    /// <see href="https://www.rfc-editor.org/rfc/rfc7807.html">RFC7807</see>.
    /// At minimum, a user should provide values for Status, Title, and Details.
    /// </summary>
    public class ApiError : IApiError
    {
        private HttpStatusCode _status;
        private string _title, _detail;
        private Uri _type, _instance;
        private IDictionary<string, object> _extensions;

        public ApiError(
            HttpStatusCode status,
            string title = null,
            string detail = null,
            Uri type = null,
            Uri instance = null,
            IDictionary<string, object> extensions = null)
        {
            Status = status;

            var baseError = BaseErrors.FromStatusCode(_status);
            if (baseError != null)
            {
                Title = title ?? baseError._title;
                Detail = detail ?? baseError._detail;
                Type = type ?? baseError._type;
                Instance = instance ?? baseError._instance;
                Extensions = extensions;
            }
            else
            {
                _title = title ?? throw new ArgumentNullException(nameof(title), "Must provide a title when no base ApiError is specified for a given HttpStatusCode");
                _detail = detail ?? throw new ArgumentNullException(nameof(detail), "Must provide details when no base ApiError is specified for a given HttpStatusCode");
                Type = type;
                Instance = instance;
                Extensions = extensions;
            }
        }

        [JsonPropertyName("status")]
        [JsonProperty(PropertyName = "status")]
        public HttpStatusCode Status
        {
            get => _status;
            private set
            {
                if (!value.IsError()) throw new ArgumentOutOfRangeException(nameof(value));
                _status = value;
            }
        }

        [JsonPropertyName("title")]
        [JsonProperty(PropertyName = "title")]
        public string Title
        {
            get => _title;
            private set => _title = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonPropertyName("detail")]
        [JsonProperty(PropertyName = "detail")]
        public string Detail
        {
            get => _detail;
            private set => _detail = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonPropertyName("type")]
        [JsonProperty(PropertyName = "type")]
        public Uri Type
        {
            get => _type;
            private set => _type = value ?? new Uri("about:blank");
        }

        [JsonPropertyName("instance")]
        [JsonProperty(PropertyName = "instance")]
        public Uri Instance
        {
            get => _instance;
            private set => _instance = value;
        }

        [System.Text.Json.Serialization.JsonExtensionData]
        [Newtonsoft.Json.JsonExtensionData]
        public IDictionary<string, object> Extensions
        {
            get => _extensions;
            private set => _extensions = value ?? new Dictionary<string, object>();
        }
    }
}
