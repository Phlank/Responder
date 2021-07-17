using System.Collections.Generic;

namespace Phlank.ApiModeling
{
    public interface IApiResponseBuilder
    {
        /// <summary>
        /// Adds an error to the <see cref="IApiResponseBuilder"/>.
        /// </summary>
        IApiResponseBuilder WithError(ApiError error);
        /// <summary>
        /// Adds errors to the <see cref="IApiResponseBuilder"/>.
        /// </summary>
        IApiResponseBuilder WithErrors(IEnumerable<ApiError> errors);
        /// <summary>
        /// Adds a warning to the <see cref="IApiResponseBuilder"/>.
        /// </summary>
        IApiResponseBuilder WithWarning(ApiWarning warning);
        /// <summary>
        /// Adds warnings to the <see cref="IApiResponseBuilder"/>.
        /// </summary>
        public IApiResponseBuilder WithWarnings(IEnumerable<ApiWarning> warnings);
        /// <summary>
        /// Creates an <see cref="ApiResponse"/> from the provided errors and warnings.
        /// </summary>
        public ApiResponse Build();
        /// <summary>
        /// Creates an <see cref="ApiResponse{TContent}"/> from the provided errors and warnings with additional content to be served to the user.
        /// </summary>
        /// <param name="content">The content to serve to the user.</param>
        public ApiResponse<TContent> Build<TContent>(TContent content) where TContent : class;
    }
}
