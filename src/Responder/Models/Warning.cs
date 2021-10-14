using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Phlank.Responder
{
    /// <summary>
    /// Information to reflect back to the user regarding any dangerous or
    /// problematic events during operation or issues with the result of the
    /// operation.
    /// </summary>
    public class Warning
    {
        private string _message;
        private IReadOnlyDictionary<string, object> _extensions;

        /// <summary>
        /// The message of the <see cref="Warning"/>.
        /// </summary>
        [JsonPropertyName("message")]
        [JsonProperty(PropertyName = "message")]
        public string Message
        {
            get => _message;
            set => _message = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// The severity of the <see cref="Warning"/>.
        /// </summary>
        [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
        [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
        [JsonPropertyName("severity")]
        [JsonProperty(PropertyName = "severity")]
        public Severity Severity { get; set; }

        /// <summary>
        /// Extension data for the <see cref="Warning"/>.
        /// </summary>
        [System.Text.Json.Serialization.JsonExtensionData]
        [Newtonsoft.Json.JsonExtensionData]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyDictionary<string, object> Extensions
        {
            get => _extensions;
            set => _extensions = value ?? new Dictionary<string, object>();
        }

        public Warning(string message, Severity severity, IReadOnlyDictionary<string, object> extensions = null)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Severity = severity;
            Extensions = extensions ?? new Dictionary<string, object>();
        }
    }
}