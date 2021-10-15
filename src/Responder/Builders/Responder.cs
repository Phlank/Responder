using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Phlank.Responder.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Phlank.Responder
{
    public class Responder : IResponder
    {
        private readonly ResponderOptions _options;

        private List<ApiError> _errors = new List<ApiError>();
        private List<Warning> _warnings = new List<Warning>();
        private List<object> _content = new List<object>();
        private HttpStatusCode _successStatusCode = HttpStatusCode.OK;

        /// <summary>
        /// Creates an instance of a <see cref="Responder"/>.
        /// </summary>
        /// <param name="options">The options injected at startup to control the behavior of the <see cref="Responder"/></param>
        public Responder(IOptions<ResponderOptions> options = null)
        {
            _options = options?.Value ?? new ResponderOptions();
        }

        public ResponderResult Build(ControllerBase controller)
        {
            var response = CreateResponse(controller);
            return new ResponderResult(response, _successStatusCode);
        }

        private Response CreateResponse(ControllerBase controller)
        {
            var output = new Response();
            if (_errors.Count > 0)
            {
                var firstError = _errors.First();
                var remainingErrors = _errors.Skip(1);

                var combinedExtensions = new Dictionary<string, object>(firstError.Extensions);
                combinedExtensions.Add("additionalErrors", remainingErrors);

                if (_options.IncludeTraceIdOnErrors) combinedExtensions["traceId"] = controller.ControllerContext.HttpContext.TraceIdentifier;

                var combinedError = new ApiError(
                    firstError.Status,
                    title: firstError.Title,
                    detail: firstError.Detail,
                    type: firstError.Type,
                    instance: firstError.Instance,
                    combinedExtensions);

                output.Error = combinedError;
            }
            else
            {
                output.Warnings = _warnings;
                if (_content.Count == 0) output.Data = null;
                else if (_content.Count == 1) output.Data = _content.First();
                else output.Data = _content;
            }

            return output;
        }

        public ResponderResult<T> Build<T>(ControllerBase controller) where T : class
        {
            var response = CreateResponse<T>(controller);
            return new ResponderResult<T>(response, _successStatusCode);
        }

        private Response<T> CreateResponse<T>(ControllerBase controller) where T : class
        {
            var output = new Response<T>();
            if (_errors.Count > 0)
            {
                var firstError = _errors.First();
                var remainingErrors = _errors.Skip(1);

                var combinedExtensions = new Dictionary<string, object>(firstError.Extensions);
                combinedExtensions.Add("additionalErrors", remainingErrors);

                if (_options.IncludeTraceIdOnErrors) combinedExtensions["traceId"] = controller.ControllerContext.HttpContext.TraceIdentifier;

                var combinedError = new ApiError(
                    firstError.Status,
                    title: firstError.Title,
                    detail: firstError.Detail,
                    type: firstError.Type,
                    instance: firstError.Instance,
                    combinedExtensions);

                output.Error = combinedError;
            }
            else
            {
                output.Warnings = _warnings;
                if (_content.Count == 0) output.Data = null;
                else if (_content.Count == 1) output.Data = _content.First() as T;
                else
                {
                    throw new Exception($"Multiple pieces of data have been added to the Responder, and they cannot all be cast together into a single instance of {typeof(T).Name}");
                }
            }

            return output;
        }

        public IResponder AddError(ApiError error)
        {
            _errors.Add(error);
            return this;
        }

        public IResponder AddError(
            HttpStatusCode status,
            string title = null,
            string detail = null,
            Uri type = null,
            Uri instance = null,
            IDictionary<string, object> extensions = null)
        {
            var error = new ApiError(status, title, detail, type, instance, extensions);
            AddError(error);
            return this;
        }

        public IResponder AddError(
            int status,
            string title = null,
            string detail = null,
            Uri type = null,
            Uri instance = null,
            IDictionary<string, object> extensions = null)
        {
            var error = new ApiError(status, title, detail, type, instance, extensions);
            AddError(error);
            return this;
        }

        public IResponder AddErrors(IEnumerable<ApiError> errors)
        {
            _errors.AddRange(errors);
            return this;
        }

        public IResponder AddProblem(ProblemDetails problem)
        {
            _errors.Add(problem.ToApiError());
            return this;
        }

        public IResponder AddProblems(IEnumerable<ProblemDetails> problems)
        {
            _errors.AddRange(problems.Select(e => e.ToApiError()));
            return this;
        }

        public IResponder AddWarning(Warning warning)
        {
            _warnings.Add(warning);
            return this;
        }

        public IResponder AddWarnings(IEnumerable<Warning> warnings)
        {
            _warnings.AddRange(warnings);
            return this;
        }

        public IResponder AddException<TException>(TException exception) where TException : Exception
        {
            _errors.Add(exception.ToApiError());
            return this;
        }

        public IResponder AddExceptions<TException>(IEnumerable<TException> exceptions) where TException : Exception
        {
            foreach (var exception in exceptions)
            {
                _errors.Add(exception.ToApiError());
            }
            return this;
        }

        public IResponder AddContent(object content)
        {
            _content.Add(content);
            return this;
        }

        public IResponder AddStatusCodeOnSuccess(HttpStatusCode successfulStatusCode)
        {
            if (successfulStatusCode.IsError())
            {
                throw new ArgumentOutOfRangeException("The provided status code must have a value less than 400.");
            }
            _successStatusCode = successfulStatusCode;
            return this;
        }

        public IResponder AddStatusCodeOnSuccess(int successfulStatusCode)
        {
            if (!Enum.IsDefined(typeof(HttpStatusCode), successfulStatusCode))
            {
                throw new ArgumentOutOfRangeException(nameof(successfulStatusCode), "The given status must have a corresponding HttpStatusCode value.");
            }

            var status = (HttpStatusCode)successfulStatusCode;
            return AddStatusCodeOnSuccess(status);
        }
    }
}
