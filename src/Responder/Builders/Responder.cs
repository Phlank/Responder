using Microsoft.AspNetCore.Http;
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

        private List<Problem> _problems = new List<Problem>();
        private Dictionary<string, object> _extensions = new Dictionary<string, object>();
        private object _content;
        private HttpStatusCode _successStatusCode = HttpStatusCode.OK;

        /// <summary>
        /// Creates an instance of a <see cref="Responder"/>.
        /// </summary>
        /// <param name="options">The options injected at startup to control the behavior of the <see cref="Responder"/></param>
        public Responder(IOptions<ResponderOptions> options = null)
        {
            _options = options?.Value ?? new ResponderOptions();
        }

        public ResponderResult<T> Build<T>(ControllerBase controller) where T : class
        {
            var response = CreateResponse<T>(controller.HttpContext);
            return new ResponderResult<T>(response, _successStatusCode);
        }

        public ResponderResult<T> Build<T>(HttpContext httpContext) where T : class
        {
            var response = CreateResponse<T>(httpContext);
            return new ResponderResult<T>(response, _successStatusCode);
        }

        private Response<T> CreateResponse<T>(HttpContext httpContext) where T : class
        {
            var output = new Response<T>();
            if (_problems.Count > 0)
            {
                var firstError = _problems.First();
                var remainingErrors = _problems.Skip(1);

                var combinedExtensions = new Dictionary<string, object>(firstError.Extensions);
                if (remainingErrors.Count() > 0)
                {
                    combinedExtensions.Add("additionalErrors", remainingErrors);
                }

                if (_options.IncludeTraceIdOnErrors) combinedExtensions["traceId"] = httpContext.TraceIdentifier;

                var combinedError = new Problem(
                    firstError.Status,
                    title: firstError.Title,
                    detail: firstError.Detail,
                    type: firstError.Type,
                    instance: firstError.Instance,
                    combinedExtensions);

                output.Problem = combinedError;
            }
            else
            {
                output.Extensions = _extensions;
                if (_content != null) output.Data = (T)_content;
            }

            return output;
        }

        public ResponderResult Build(ControllerBase controller)
        {
            return Build(controller.HttpContext);
        }

        public ResponderResult Build(HttpContext httpContext)
        {
            var response = CreateResponse(httpContext);
            return new ResponderResult(response, _successStatusCode);
        }

        private Response CreateResponse(HttpContext httpContext)
        {
            var output = new Response();
            if (_problems.Count > 0)
            {
                var firstError = _problems.First();
                var remainingErrors = _problems.Skip(1);

                var combinedExtensions = new Dictionary<string, object>(firstError.Extensions);
                if (remainingErrors.Count() > 0)
                {
                    combinedExtensions.Add("additionalErrors", remainingErrors);
                }

                if (_options.IncludeTraceIdOnErrors) combinedExtensions["traceId"] = httpContext.TraceIdentifier;

                var combinedError = new Problem(
                    firstError.Status,
                    title: firstError.Title,
                    detail: firstError.Detail,
                    type: firstError.Type,
                    instance: firstError.Instance,
                    combinedExtensions);

                output.Problem = combinedError;
            }
            else
            {
                output.Extensions = _extensions;
            }

            return output;
        }

        public IResponder AddProblem(Problem problem)
        {
            _problems.Add(problem);
            return this;
        }

        public IResponder AddProblem(
            HttpStatusCode status,
            string title = null,
            string detail = null,
            Uri type = null,
            Uri instance = null,
            IDictionary<string, object> extensions = null)
        {
            var error = new Problem(status, title, detail, type, instance, extensions);
            AddProblem(error);
            return this;
        }

        public IResponder AddProblem(
            int status,
            string title = null,
            string detail = null,
            Uri type = null,
            Uri instance = null,
            IDictionary<string, object> extensions = null)
        {
            var error = new Problem(status, title, detail, type, instance, extensions);
            AddProblem(error);
            return this;
        }

        public IResponder AddProblems(IEnumerable<Problem> problems)
        {
            _problems.AddRange(problems);
            return this;
        }

        public IResponder AddProblem(ProblemDetails problem)
        {
            var error = problem.ToProblem();
            return AddProblem(error);
        }

        public IResponder AddProblems(IEnumerable<ProblemDetails> problems)
        {
            var errors = problems.Select(e => e.ToProblem());
            return AddProblems(errors);
        }

        public IResponder AddException<TException>(TException exception) where TException : Exception
        {
            _problems.Add(exception.ToProblem());
            return this;
        }

        public IResponder AddExceptions<TException>(IEnumerable<TException> exceptions) where TException : Exception
        {
            foreach (var exception in exceptions)
            {
                _problems.Add(exception.ToProblem());
            }
            return this;
        }

        public IResponder AddContent(object content)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            if (_content != null) throw new InvalidOperationException("Content has already been provided for this responder and cannot be overwritten.");
            _content = content;
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

        public IResponder AddExtension(string key, object value)
        {
            _extensions.Add(key, value);
            return this;
        }

        public IResponder AddExtension(KeyValuePair<string, object> keyValuePair)
        {
            _extensions.Add(keyValuePair.Key, keyValuePair.Value);
            return this;
        }

        public IResponder AddExtensions(IEnumerable<KeyValuePair<string, object>> extensions)
        {
            foreach (var extension in extensions)
            {
                _extensions.Add(extension.Key, extension.Value);
            }
            return this;
        }

        public IResponder AddExtensions(IDictionary<string, object> extensions)
        {
            foreach (var extension in extensions)
            {
                _extensions.Add(extension.Key, extension.Value);
            }
            return this;
        }
    }
}
