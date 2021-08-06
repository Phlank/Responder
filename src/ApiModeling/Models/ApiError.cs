﻿using Newtonsoft.Json;
using Phlank.ApiModeling.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace Phlank.ApiModeling
{
    /// <summary>
    /// An error to be reflected by the API. This class follows the
    /// specification in
    /// <see href="https://www.rfc-editor.org/rfc/rfc7807.html">RFC7807</see>.
    /// At minimum, a user should provide values for Status, Title, and Details.
    /// </summary>
    public class ApiError
    {
        private static readonly Uri BlankType = new Uri("about:blank");
        private static readonly Uri BadRequestType = new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1");
        private static readonly Uri InternalServerErrorType = new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1");
        private static readonly Uri NotImplementedType = new Uri("https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.2");

        private HttpStatusCode _status = HttpStatusCode.InternalServerError;
        private Uri _type;

        /// <summary>
        /// The HTTP status code generated by the origin server for this
        /// occurence of the problem.
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public HttpStatusCode Status
        {
            get => _status;
            set
            {
                if (!value.IsError())
                {
                    throw new ArgumentOutOfRangeException("Status must have a value greater than or equal to 400.");
                }
                else
                {
                    _status = value;
                }
            }
        }

        /// <summary>
        /// A URI reference that identifies the problem type. When this member
        /// is not specified, it will use a default <see cref="Uri"/> pointing
        /// to the RFC documentation of this error's Status.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        [JsonPropertyName("type")]
        public Uri Type
        {
            get
            {
                if (_type != default)
                {
                    return _type;
                }
                else
                {
                    return _status switch
                    {
                        HttpStatusCode.BadRequest => BadRequestType,
                        HttpStatusCode.InternalServerError => InternalServerErrorType,
                        HttpStatusCode.NotImplemented => NotImplementedType,
                        _ => BlankType,
                    };
                }
            }
            set
            {
                _type = value;
            }
        }

        /// <summary>
        /// A short, human-readable summary of the problem type. It SHOULD NOT
        /// change from occurrence to occurrence of the problem, except for
        /// purposes of localization.
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// A human-readable explanation specific to this occurrence of the
        /// problem.
        /// </summary>
        [JsonProperty(PropertyName = "detail")]
        [JsonPropertyName("detail")]
        public string Detail { get; set; }

        /// <summary>
        /// A URI reference that identifies the specific occurrence of the
        /// problem. It may or may not yield further information if
        /// dereferenced.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "instance")]
#if NET5_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        [JsonPropertyName("instance")]
        public Uri Instance { get; set; }

        /// <summary>
        /// Additional information to reflect back to the client. These items
        /// will be surfaced in the top level of the serialized response.
        /// </summary>
        [Newtonsoft.Json.JsonExtensionData]
        [System.Text.Json.Serialization.JsonExtensionData]
        public Dictionary<string, object> Extensions { get; set; } = new Dictionary<string, object>();
    }
}
