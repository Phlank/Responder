using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Phlank.ApiModeling
{
    public class ApiWarning
    {
        public IEnumerable<string> Fields { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
        public Severity Severity { get; set; }
    }
}