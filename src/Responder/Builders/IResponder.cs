using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace Phlank.Responder
{
    /// <summary>
    /// An instance of IResponder is used to build a <see cref="ResponderResult"/>.
    /// </summary>
    public interface IResponder
    {
        /// <summary>
        /// Adds a problem to the IResponder.
        /// </summary>
        /// <param name="problem">The problem to add.</param>
        /// <returns>The IResponder instance.</returns>
        IResponder AddProblem(Problem problem);

        /// <summary>
        /// Creates a <see cref="Problem"/> from the arguments and adds it to the IResponder.
        /// </summary>
        /// <param name="status">The status of the problem.</param>
        /// <param name="title">The title of the problem.</param>
        /// <param name="detail">The detail of the problem.</param>
        /// <param name="type">The type of the problem.</param>
        /// <param name="instance">The instance of the problem.</param>
        /// <param name="extensions"></param>
        IResponder AddProblem(
            HttpStatusCode status,
            string title = null,
            string detail = null,
            Uri type = null,
            Uri instance = null,
            IDictionary<string, object> extensions = null);

        /// <summary>
        /// Creates an error from the arguments and adds it to the IResponder.
        /// </summary>
        IResponder AddProblem(
            int status,
            string title = null,
            string detail = null,
            Uri type = null,
            Uri instance = null,
            IDictionary<string, object> extensions = null);

        /// <summary>
        /// Adds the <see cref="ProblemDetails" /> to the IResponder.
        /// </summary>
        /// <param name="problem">The ProblemDetails to add.</param>
        /// <returns>The IResponder instance.</returns>
        IResponder AddProblem(ProblemDetails problem);

        /// <summary>
        /// Adds errors to the IResponder.
        /// </summary>
        IResponder AddProblems(IEnumerable<Problem> problems);

        /// <summary>
        /// Adds <see cref="ProblemDetails"/> to the IResponder.
        /// </summary>
        IResponder AddProblems(IEnumerable<ProblemDetails> problems);

        /// <summary>
        /// Adds a single extension to the IResponder.
        /// </summary>
        /// <param name="key">The key of the extension data.</param>
        /// <param name="value">The value of the extension data.</param>
        /// <returns>The IResponder instance.</returns>
        IResponder AddExtension(string key, object value);

        IResponder AddExtension(KeyValuePair<string, object> keyValuePair);

        IResponder AddExtensions(IEnumerable<KeyValuePair<string, object>> extensions);

        IResponder AddExtensions(IDictionary<string, object> extensions);

        /// <summary>
        /// Adds an exception to the IResponder as a problem.
        /// </summary>
        IResponder AddException<TException>(TException exception) where TException : Exception;

        /// <summary>
        /// Adds exceptions to the IResponder as problems.
        /// </summary>
        IResponder AddExceptions<TException>(IEnumerable<TException> exception) where TException : Exception;

        /// <summary>
        /// Adds content to the IResponder
        /// </summary>
        IResponder AddContent(object content);

        /// <summary>
        /// Adds a status code to return to the IResponder if the
        /// operation is successful. Using this method multiple times will
        /// replace previously added status codes. The default successful
        /// status code is <see cref="HttpStatusCode.OK"/>.
        /// </summary>
        /// <param name="statusCode">The status code to return to the client if the operation is successful.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="statusCode"/> provided has a value greater than or equal to 400.</exception>
        IResponder AddStatusCodeOnSuccess(HttpStatusCode statusCode);

        /// <summary>
        /// Adds a status code to return to the IResponder if the
        /// operation is successful. Using this method multiple times will
        /// replace previously added status codes. The default successful 
        /// status code is 200.
        /// </summary>
        /// <param name="statusCode">The integer representation of the status code to return to the client if the operation is successful.</param>
        /// <exception cref="ArgumentException">An <see cref="ArgumentException"/> may be thrown under two circumstances; first, if the provided <paramref name="statusCode"/> has no corresponding <see cref="HttpStatusCode"/>, and second, if the provided <paramref name="statusCode"/> has a matching <see cref="HttpStatusCode"/> but it is an erroring status code.</exception>
        IResponder AddStatusCodeOnSuccess(int statusCode);

        /// <summary>
        /// Creates a <see cref="ResponderResult{T}"/> from the provided content, problems, and extensions.
        /// </summary>
        /// <typeparam name="T">The type of the data to return.</typeparam>
        /// <param name="controller">The controller making use of the IResponder.</param>
        /// <exception cref="InvalidCastException">Thrown if the content provided cannot be cast into the given type.</exception>
        ResponderResult<T> Build<T>(ControllerBase controller) where T : class;

        /// <summary>
        /// Creates a <see cref="ResponderResult{T}"/> from the provided problems, content, and extensions.
        /// </summary>
        /// <typeparam name="T">The type of the content included in the <see cref="ResponderResult{T}"/>.</typeparam>
        /// <param name="httpContext">The context of the current action.</param>
        /// <returns></returns>
        ResponderResult<T> Build<T>(HttpContext httpContext) where T : class;

        ResponderResult Build(ControllerBase controller);

        ResponderResult Build(HttpContext httpContext);
    }
}
