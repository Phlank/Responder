using System.Collections.Generic;

namespace Phlank.ApiModeling
{
    /// <summary>
    /// A response to send to a client.
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Warnings regarding the attempted operation.
        /// </summary>
        public IEnumerable<ApiWarning> Warnings { get; internal set; }
        /// <summary>
        /// The content to return back to the user as a result of the operation.
        /// </summary>
        public object Content { get; internal set; }
    }
}
