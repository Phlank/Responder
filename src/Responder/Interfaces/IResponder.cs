using System;
using System.Collections.Generic;
using System.Net;

namespace Phlank.Responder
{
    /// <summary>
    /// An instance of IResponder is used to build an <see cref="ApiResult"/>.
    /// </summary>
    public interface IResponder
    {
        /// <summary>
        /// Adds an error to the <see cref="IResponder"/>.
        /// </summary>
        IResponder WithError(ApiError error);

        /// <summary>
        /// Adds errors to the <see cref="IResponder"/>.
        /// </summary>
        IResponder WithErrors(IEnumerable<ApiError> errors);

        /// <summary>
        /// Adds a warning to the <see cref="IResponder"/>.
        /// </summary>
        IResponder WithWarning(ApiWarning warning);

        /// <summary>
        /// Adds warnings to the <see cref="IResponder"/>.
        /// </summary>
        IResponder WithWarnings(IEnumerable<ApiWarning> warnings);

        /// <summary>
        /// Adds an exception to the <see cref="IResponder"/> as an error.
        /// </summary>
        IResponder WithException<TException>(TException exception) where TException : Exception;

        /// <summary>
        /// Adds exceptions to the <see cref="IResponder"/> as errors.
        /// </summary>
        IResponder WithExceptions<TException>(IEnumerable<TException> exception) where TException : Exception;

        /// <summary>
        /// Adds content to the <see cref="IResponder"/>
        /// </summary>
        IResponder WithContent(object content);

        /// <summary>
        /// Adds a status code to return to the <see cref="IResponder"/> if the operation is successful.
        /// </summary>
        IResponder WithStatusCodeOnSuccess(HttpStatusCode statusCode);

        /// <summary>
        /// Creates an <see cref="ApiResponse"/> from the provided errors and warnings.
        /// </summary>
        ApiResult Build();
    }
}
