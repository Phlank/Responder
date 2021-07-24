using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Phlank.ApiModeling
{
    internal class ApiResultBuilder : IApiResultBuilder
    {
        private List<ApiError> _errors = new List<ApiError>();
        private List<ApiWarning> _warnings = new List<ApiWarning>();
        private object _content;

        public ApiResultBuilder() { }

        public ApiResult Build()
        {
            var responseModel = new ApiResponse
            {
                Errors = _errors,
                Warnings = _warnings,
                Content = _content
            };
            return new ApiResult(responseModel);
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
    }
}
