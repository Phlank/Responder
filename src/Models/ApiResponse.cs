using System.Collections.Generic;
using System.Linq;

namespace Phlank.ApiModeling
{
    public class ApiResponse
    {
        public bool Success { get => Errors == null || Errors.Count() == 0; }

        public IEnumerable<ApiError> Errors { get; }
        public IEnumerable<ApiWarning> Warnings { get; }

        internal ApiResponse(
            IEnumerable<ApiError> errors,
            IEnumerable<ApiWarning> warnings)
        {
            Errors = errors;
            Warnings = warnings;
        }
    }

    public class ApiResponse<TContent> where TContent : class
    {
        public bool Success { get => Errors == null || Errors.Count() == 0; }

        public IEnumerable<ApiError> Errors { get; }
        public IEnumerable<ApiWarning> Warnings { get; }
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
