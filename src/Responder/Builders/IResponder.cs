using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace Phlank.Responder
{
    /// <summary>
    /// An instance of IResponder is used to build an <see cref="ResponderResult"/>.
    /// </summary>
    public interface IResponder
    {
        /// <summary>
        /// Adds an error to the <see cref="IResponder"/>.
        /// </summary>
        IResponder AddError(ApiError error);

        /// <summary>
        /// Creates an error from the arguments and adds it to the 
        /// <see cref="IResponder"/>.
        /// </summary>
        IResponder AddError(
            HttpStatusCode status, 
            string title = null, 
            string detail = null, 
            Uri type = null, 
            Uri instance = null, 
            IDictionary<string, object> extensions = null);

        /// <summary>
        /// Creates an error from the arguments and adds it to the 
        /// <see cref="IResponder"/>.
        /// </summary>
        IResponder AddError(
            int status,
            string title = null,
            string detail = null,
            Uri type = null,
            Uri instance = null,
            IDictionary<string, object> extensions = null);

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
        IResponder AddWarning(Warning warning);

        /// <summary>
        /// Adds warnings to the <see cref="IResponder"/>.
        /// </summary>
        IResponder AddWarnings(IEnumerable<Warning> warnings);

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
        /// replace previously added status codes. The default successful
        /// status code is <see cref="HttpStatusCode.OK"/>.
        /// </summary>
        IResponder AddStatusCodeOnSuccess(HttpStatusCode statusCode);

        /// <summary>
        /// Adds a status code to return to the <see cref="IResponder"/> if the
        /// operation is successful. Using this method multiple times will
        /// replace previously added status codes. The default successful 
        /// status code is 200.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">An <see cref="ArgumentOutOfRangeException"/> may be thrown under two circumstances; first, if the provided <paramref name="statusCode"/> has no corresponding <see cref="HttpStatusCode"/>, and second, if the provided <paramref name="statusCode"/> has a matching <see cref="HttpStatusCode"/> but it is an erroring status code.</exception>
        IResponder AddStatusCodeOnSuccess(int statusCode);

        /// <summary>
        /// Creates a <see cref="Response"/> from the provided errors and
        /// warnings.
        /// </summary>
        ResponderResult Build(ControllerBase controller);

        /// <summary>
        /// Creates a <see cref="ResponderResult{T}"/> from the provided errors
        /// and warnings.
        /// </summary>
        /// <typeparam name="T">The type of the data to return.</typeparam>
        ResponderResult<T> Build<T>(ControllerBase controller) where T : class;
    }
}
