using Microsoft.AspNetCore.Mvc;
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
        IResponder AddError(ApiError error);

        /// <summary>
        /// Adds errors to the <see cref="IResponder"/>.
        /// </summary>
        IResponder AddErrors(IEnumerable<ApiError> errors);

        /// <summary>
        /// Adds a <see cref="ProblemDetails">ProblemDetail</see> to the <see cref="IResponder"/>
        /// </summary>
        IResponder AddProblem(ProblemDetails problem);

        /// <summary>
        /// Adds <see cref="ProblemDetails"/> to the <see cref="IResponder"/>
        /// </summary>
        IResponder AddProblems(IEnumerable<ProblemDetails> problems);

        /// <summary>
        /// Adds a warning to the <see cref="IResponder"/>.
        /// </summary>
        IResponder AddWarning(ApiWarning warning);

        /// <summary>
        /// Adds warnings to the <see cref="IResponder"/>.
        /// </summary>
        IResponder AddWarnings(IEnumerable<ApiWarning> warnings);

        /// <summary>
        /// Adds an exception to the <see cref="IResponder"/> as an error.
        /// </summary>
        IResponder AddException<TException>(TException exception) where TException : Exception;

        /// <summary>
        /// Adds exceptions to the <see cref="IResponder"/> as errors.
        /// </summary>
        IResponder AddExceptions<TException>(IEnumerable<TException> exception) where TException : Exception;

        /// <summary>
        /// Adds content to the <see cref="IResponder"/>
        /// </summary>
        IResponder AddContent(object content);

        /// <summary>
        /// Adds a status code to return to the <see cref="IResponder"/> if the
        /// operation is successful. Using this method multiple times will
        /// replace previously added status codes.
        /// </summary>
        IResponder AddStatusCodeOnSuccess(HttpStatusCode statusCode);

        /// <summary>
        /// Creates an <see cref="ApiResponse"/> from the provided errors and
        /// warnings.
        /// </summary>
        ApiResult Build();
    }
}
