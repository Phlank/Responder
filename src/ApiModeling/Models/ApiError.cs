using System.Collections.Generic;

namespace Phlank.ApiModeling
{
    /// <summary>
    /// Information to reflect back to the user regarding the failure of an attempted operation.
    /// </summary>
    public class ApiError
    {
        /// <summary>
        /// The names of the fields that are involved in the failure of the operation.
        /// </summary>
        public IEnumerable<string> Fields { get; set; }
        /// <summary>
        /// The error code involved in the failure of the operation.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Further information regarding the failure of the operation. This should include some type of corrective measure that will prevent failure by the same means.
        /// </summary>
        public string Message { get; set; }

        public ApiError()
        {
        }
    }
}