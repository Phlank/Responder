using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Phlank.ApiModeling
{
    /// <summary>
    /// Information to reflect back to the user regarding any dangerous or problematic events during operation or issues with the result of the operation.
    /// </summary>
    public class ApiWarning
    {
        /// <summary>
        /// The names of the fields involved with the operation warning.
        /// </summary>
        public IEnumerable<string> Fields { get; set; }
        /// <summary>
        /// The warning code involved in the operation warning.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Further information regarding the operation warning. This should include some type of corrective measure that will prevent warnings by the same means.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// The severity of the warning.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
        public Severity Severity { get; set; }

        public ApiWarning()
        {
        }
    }
}