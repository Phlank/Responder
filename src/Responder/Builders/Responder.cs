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
        /// <param name="options"></param>
        public Responder(IOptions<ResponderOptions> options)
        {
            _options = options.Value;
        }

        public ResponderResult Build()
        {
            if (_errors.Count > 0)
            {
                return new ResponderResult(CreateErrorValue())
                {
                    StatusCode = (int)_errors.First().Status,
                    ContentType = $"application/problem+json; charset={_options.CharSet}"
                };
            }
            else
            {
                return new ResponderResult(CreateSuccessValue())
                {
                    StatusCode = (int)_successStatusCode,
                    ContentType = $"application/json; charset={_options.CharSet}"
                };
            }
        }

        private ApiError CreateErrorValue()
        {
            var firstError = _errors.First();
            var remainingErrors = _errors.GetRange(1, _errors.Count - 1);

            var extensions = firstError.Extensions ?? new Dictionary<string, object>();

            if (remainingErrors.Count() > 0)
            {
                extensions.Add("otherErrors", remainingErrors);
            }

            return new ApiError
            {
                Detail = firstError.Detail,
                Instance = firstError.Instance,
                Status = firstError.Status,
                Title = firstError.Title,
                Type = firstError.Type,
                Extensions = extensions
            };
        }

        private ApiModel CreateSuccessValue()
        {
            object content = null;
            if (_content.Count() == 1)
            {
                content = _content.First();
            }
            else if (_content.Count() > 1)
            {
                content = _content;
            }

            return new ApiModel
            {
                Content = content,
                Warnings = _warnings
            };
        }

        public IResponder AddError(ApiError error)
        {
            _errors.Add(error);
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
            if (!successfulStatusCode.IsSuccessful())
            {
                throw new ArgumentOutOfRangeException("The provided status code must have a value between 200 and 299.");
            }
            _successStatusCode = successfulStatusCode;
            return this;
        }
    }
}
