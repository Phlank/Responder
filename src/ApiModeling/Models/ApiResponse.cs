using System.Collections.Generic;
using System.Linq;

namespace Phlank.ApiModeling
{
    /// <summary>
    /// A response to send to a client.
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// True if no errors are given for the <see cref="ApiResponse"/>; otherwise, false.
        /// </summary>
        public bool Success { get => Errors == null || Errors.Count() == 0; }
        /// <summary>
        /// Errors preventing success of the attempted operation.
        /// </summary>
        public IEnumerable<ApiError> Errors { get; internal init; }
        /// <summary>
        /// Warnings regarding the attempted operation.
        /// </summary>
        public IEnumerable<ApiWarning> Warnings { get; internal init; }
        /// <summary>
        /// The content to return back to the user as a result of the operation.
        /// </summary>
        public object Content { get; internal init; }
    }
}
