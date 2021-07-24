using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Phlank.ApiModeling
{
    public interface IApiResultBuilder
    {
        /// <summary>
        /// Adds an error to the <see cref="IApiResultBuilder"/>.
        /// </summary>
        IApiResultBuilder WithError(ApiError error);
        /// <summary>
        /// Adds errors to the <see cref="IApiResultBuilder"/>.
        /// </summary>
        IApiResultBuilder WithErrors(IEnumerable<ApiError> errors);
        /// <summary>
        /// Adds a warning to the <see cref="IApiResultBuilder"/>.
        /// </summary>
        IApiResultBuilder WithWarning(ApiWarning warning);
        /// <summary>
        /// Adds warnings to the <see cref="IApiResultBuilder"/>.
        /// </summary>
        IApiResultBuilder WithWarnings(IEnumerable<ApiWarning> warnings);
        /// <summary>
        /// Adds content to the <see cref="IApiResultBuilder"/>
        /// </summary>
        IApiResultBuilder WithContent(object content);
        /// <summary>
        /// Creates an <see cref="ApiResponse"/> from the provided errors and warnings.
        /// </summary>
        ApiResult Build();
    }
}
