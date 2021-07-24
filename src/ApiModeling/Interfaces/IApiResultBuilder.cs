using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

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
        /// Adds a status code to return to the <see cref="IApiResultBuilder"> if the operation is successful.
        /// </summary>
        IApiResultBuilder WithStatusCodeOnSuccess(HttpStatusCode statusCode);
        /// <summary>
        /// Creates an <see cref="ApiResponse"/> from the provided errors and warnings.
        /// </summary>
        ApiResult Build();
    }
}
