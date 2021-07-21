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
        /// Errors preventing success of the attempted operations.
        /// </summary>
        public IEnumerable<ApiError> Errors { get; }
        /// <summary>
        /// Warnings regarding the attempted operation.
        /// </summary>
        public IEnumerable<ApiWarning> Warnings { get; }

        internal ApiResponse(
            IEnumerable<ApiError> errors,
            IEnumerable<ApiWarning> warnings)
        {
            Errors = errors;
            Warnings = warnings;
        }
    }

    /// <summary>
    /// A response to send to a client with content attached to it.
    /// </summary>
    /// <typeparam name="TContent">The type of the object used as content.</typeparam>
    public class ApiResponse<TContent> where TContent : class
    {
        /// <summary>
        /// True if no errors are given for the <see cref="ApiResponse{TContent}"/>; otherwise, false.
        /// </summary>
        public bool Success { get => Errors == null || Errors.Count() == 0; }
        /// <summary>
        /// Errors preventing success of the attempted operations.
        /// </summary>
        public IEnumerable<ApiError> Errors { get; }
        /// <summary>
        /// Warnings regarding the attempted operations.
        /// </summary>
        public IEnumerable<ApiWarning> Warnings { get; }
        /// <summary>
        /// Content resulting from the attempted operations.
        /// </summary>
        public TContent Content { get; }

        internal ApiResponse(
            IEnumerable<ApiError> errors,
            IEnumerable<ApiWarning> warnings,
            TContent content)
        {
            Errors = errors;
            Warnings = warnings;
            Content = content;
        }
    }
}
