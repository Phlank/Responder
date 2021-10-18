using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace Phlank.Responder
{
    internal class SerializableResponse
    {
        public bool IsSuccessful { get; set; }

        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Detail { get; set; }

        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri Instance { get; set; }

        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public HttpStatusCode? Status { get; set; }

        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Title { get; set; }

        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri Type { get; set; }

        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonExtensionData]
        [Newtonsoft.Json.JsonExtensionData]
        public IDictionary<string, object> Extensions { get; set; }
    }

    internal class SerializableResponse<T> : SerializableResponse
    {
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public T Data { get; set; }
    }
}
