using Microsoft.Extensions.Options;
using Phlank.Responder.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Phlank.Responder
{
    internal class Responder : IResponder
    {
        private readonly ResponderOptions _options;

        private readonly List<ApiError> _errors = new List<ApiError>();
        private List<ApiWarning> _warnings = new List<ApiWarning>();
        private object _content;
        private HttpStatusCode _successStatusCode = HttpStatusCode.OK;

        public Responder(IOptions<ResponderOptions> options)
        {
            _options = options.Value;
        }

        public ApiResult Build()
        {
            if (_errors.Count > 0)
            {
                return new ApiResult(CreateErrorValue())
                {
                    StatusCode = (int)_errors.First().Status,
                    ContentType = $"application/problem+json; charset={_options.CharSet}"
                };
            }
            else
            {
                return new ApiResult(CreateSuccessValue())
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

        private ApiResponse CreateSuccessValue()
        {
            return new ApiResponse
            {
                Content = _content,
                Warnings = _warnings
            };
        }

        public IResponder WithError(ApiError error)
        {
            _errors.Add(error);
            return this;
        }

        public IResponder WithErrors(IEnumerable<ApiError> errors)
        {
            _errors.AddRange(errors);
            return this;
        }

        public IResponder WithWarning(ApiWarning warning)
        {
            if (_warnings == null) _warnings = new List<ApiWarning>();
            _warnings.Add(warning);
            return this;
        }

        public IResponder WithWarnings(IEnumerable<ApiWarning> warnings)
        {
            if (_warnings == null) _warnings = new List<ApiWarning>();
            _warnings.AddRange(warnings);
            return this;
        }

        public IResponder WithException<TException>(TException exception) where TException : Exception
        {
            _errors.Add(exception.ToApiError());
            return this;
        }

        public IResponder WithExceptions<TException>(IEnumerable<TException> exceptions) where TException : Exception
        {
            foreach (var exception in exceptions)
            {
                _errors.Add(exception.ToApiError());
            }
            return this;
        }

        public IResponder WithContent(object content)
        {
            _content = content;
            return this;
        }

        public IResponder WithStatusCodeOnSuccess(HttpStatusCode successfulStatusCode)
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
