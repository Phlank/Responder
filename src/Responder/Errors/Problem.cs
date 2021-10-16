using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Phlank.Responder.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;
using System.Linq;
using System.Collections.ObjectModel;

namespace Phlank.Responder
{
    /// <summary>
    /// An error to be reflected by the API. This class follows the
    /// specification in
    /// <see href="https://www.rfc-editor.org/rfc/rfc7807.html">RFC7807</see>.
    /// At minimum, a user should provide values for Status, Title, and Details.
    /// </summary>
    public class Problem : IProblem
    {
        private HttpStatusCode _status;
        private string _title, _detail;
        private Uri _type, _instance;
        private IDictionary<string, object> _extensions;

        /// <summary>
        /// Creates an <see cref="Problem"/> from the given arguments. 
        /// <paramref name="status" /> is the only required parameter in most 
        /// circumstances. If a field is not provided, then the default value
        /// from a base list of <see cref="Problem">Problems</see> will be 
        /// used.
        /// </summary>
        /// <param name="status">The <see cref="HttpStatusCode"/> relating to the error.</param>
        /// <param name="title">The title of the error. If none is provided, a default value will be used.</param>
        /// <param name="detail">The detail text of the error. If none is provided, a default value will be used.</param>
        /// <param name="type">The URI type reference of the error. If none is provided, a default value will be used.</param>
        /// <param name="instance">The instance of the specific context relating to the Problem. If left blank, this will be provided for by the <see cref="HttpContext"/> belonging to the <see cref="ControllerContext"/>.</param>
        /// <param name="extensions">Additional information relating to the error that has occured.</param>
        /// <exception cref="ArgumentNullException">If no default <see cref="Problem"/> is found matching the provided <paramref name="status"/>, and either <paramref name="title"/> or <paramref name="detail"/> are null, an <see cref="ArgumentNullException"/> will be thrown.</exception>
        public Problem(
            HttpStatusCode status,
            string title = null,
            string detail = null,
            Uri type = null,
            Uri instance = null,
            IDictionary<string, object> extensions = null)
        {
            Status = status;

            EvaluatePropertiesForArguments(title, detail, type, instance, extensions);
        }

        /// <summary>
        /// Creates an <see cref="Problem"/> from the given arguments. 
        /// <paramref name="status" /> is the only required parameter in most 
        /// circumstances. If a field is not provided, then the default value
        /// from a base list of <see cref="Problem">Problems</see> will be 
        /// used.
        /// </summary>
        /// <param name="status">The status code relating to the error.</param>
        /// <param name="title">The title of the error. If none is provided, a default value will be used.</param>
        /// <param name="detail">The detail text of the error. If none is provided, a default value will be used.</param>
        /// <param name="type">The URI type reference of the error. If none is provided, a default value will be used.</param>
        /// <param name="instance">The instance of the specific context relating to the Problem. If left blank, this will be provided for by the <see cref="HttpContext"/> belonging to the <see cref="ControllerContext"/>.</param>
        /// <param name="extensions">Additional information relating to the error that has occured.</param>
        /// <exception cref="ArgumentNullException">If no default <see cref="Problem"/> is found matching the provided <paramref name="status"/>, and either <paramref name="title"/> or <paramref name="detail"/> are null, an <see cref="ArgumentNullException"/> will be thrown.</exception>
        /// <exception cref="ArgumentOutOfRangeException">An <see cref="ArgumentOutOfRangeException"/> may be thrown under two circumstances; first, if the provided <paramref name="status"/> has no corresponding <see cref="HttpStatusCode"/>, and second, if the provided <paramref name="status"/> has a matching <see cref="HttpStatusCode"/> but it is not a valid erroring status code.</exception>
        public Problem(
            int status,
            string title = null,
            string detail = null,
            Uri type = null,
            Uri instance = null,
            IDictionary<string, object> extensions = null)
        {
            if (!Enum.IsDefined(typeof(HttpStatusCode), status)) throw new ArgumentOutOfRangeException(nameof(status), "The given status must have a corresponding HttpStatusCode value.");
            Status = (HttpStatusCode)status;

            EvaluatePropertiesForArguments(title, detail, type, instance, extensions);
        }

        private void EvaluatePropertiesForArguments(string title, string detail, Uri type, Uri instance, IDictionary<string, object> extensions)
        {
            extensions ??= new Dictionary<string, object>();

            var baseError = BaseErrors.FromStatusCode(_status);
            if (baseError != null)
            {
                Title = title ?? baseError._title;
                Detail = detail ?? baseError._detail;
                Type = type ?? baseError._type;
                Instance = instance ?? baseError._instance;
                Extensions = extensions != default ? new Dictionary<string, object>(extensions) : new Dictionary<string, object>();
            }
            else
            {
                _title = title ?? throw new ArgumentNullException(nameof(title), "Must provide a title when no base Problem is specified for a given HttpStatusCode");
                _detail = detail ?? throw new ArgumentNullException(nameof(detail), "Must provide details when no base Problem is specified for a given HttpStatusCode");
                Type = type;
                Instance = instance;
                Extensions = extensions != default ? new Dictionary<string, object>(extensions) : new Dictionary<string, object>();
            }
        }

        [JsonPropertyName("status")]
        [JsonProperty(PropertyName = "status")]
        public HttpStatusCode Status
        {
            get => _status;
            set
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
            set => _title = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonPropertyName("detail")]
        [JsonProperty(PropertyName = "detail")]
        public string Detail
        {
            get => _detail;
            set => _detail = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonPropertyName("type")]
        [JsonProperty(PropertyName = "type", DefaultValueHandling = DefaultValueHandling.Ignore)]
#if NET5_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public Uri Type
        {
            get => _type;
            set => _type = value ?? _type ?? new Uri("about:blank");
        }

        [JsonPropertyName("instance")]
        [JsonProperty(PropertyName = "instance", DefaultValueHandling = DefaultValueHandling.Ignore)]
#if NET5_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public Uri Instance
        {
            get => _instance;
            set => _instance = value;
        }

        [System.Text.Json.Serialization.JsonExtensionData]
        [Newtonsoft.Json.JsonExtensionData]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
#if NET5_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public IDictionary<string, object> Extensions
        {
            get => _extensions;
            set => _extensions = value ?? new Dictionary<string, object>();
        }
    }
}
