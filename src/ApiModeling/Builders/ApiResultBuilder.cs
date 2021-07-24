using Phlank.ApiModeling.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Phlank.ApiModeling
{
    internal class ApiResultBuilder : IApiResultBuilder
    {
        private List<ApiError> _errors = new List<ApiError>();
        private List<ApiWarning> _warnings = new List<ApiWarning>();
        private object _content;
        private HttpStatusCode _successStatusCode = HttpStatusCode.OK;

        public ApiResultBuilder() { }

        public ApiResult Build()
        {
            var responseModel = new ApiResponse
            {
                Errors = _errors,
                Warnings = _warnings,
                Content = _content
            };
            var result = new ApiResult(responseModel);
            if (_errors.Count() == 0)
            {
                result.StatusCode = (int)_successStatusCode;
            }
            return result;
        }

        public IApiResultBuilder WithError(ApiError error)
        {
            if (_errors == null) _errors = new List<ApiError>();
            _errors.Add(error);
            return this;
        }

        public IApiResultBuilder WithErrors(IEnumerable<ApiError> errors)
        {
            if (_errors == null) _errors = new List<ApiError>();
            _errors.AddRange(errors);
            return this;
        }

        public IApiResultBuilder WithWarning(ApiWarning warning)
        {
            if (_warnings == null) _warnings = new List<ApiWarning>();
            _warnings.Add(warning);
            return this;
        }

        public IApiResultBuilder WithWarnings(IEnumerable<ApiWarning> warnings)
        {
            if (_warnings == null) _warnings = new List<ApiWarning>();
            _warnings.AddRange(warnings);
            return this;
        }

        public IApiResultBuilder WithContent(object content)
        {
            _content = content;
            return this;
        }

        public IApiResultBuilder WithStatusCodeOnSuccess(HttpStatusCode successfulStatusCode)
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
