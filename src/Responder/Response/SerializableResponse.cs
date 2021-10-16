using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Phlank.Responder
{
    internal class SerializableResponse<T> where T : class
    {
        public bool IsSuccessful { get; set; }

#if NET5_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public T Data { get; set; }

#if NET5_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Detail { get; set; }

#if NET5_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri Instance { get; set; }

#if NET5_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public HttpStatusCode? Status { get; set; }

#if NET5_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Title { get; set; }

#if NET5_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri Type { get; set; }

#if NET5_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, object> Extensions { get; set; }
    }
}
