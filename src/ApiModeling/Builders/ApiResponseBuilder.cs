using System.Collections.Generic;

namespace Phlank.ApiModeling
{
    internal class ApiResponseBuilder : IApiResponseBuilder
    {
        private List<ApiError> _errors;
        private List<ApiWarning> _warnings;

        public ApiResponseBuilder() { }

        public ApiResponse Build()
        {
            return new ApiResponse(_errors, _warnings);
        }

        public ApiResponse<TContent> Build<TContent>(TContent content) where TContent : class
        {
            return new ApiResponse<TContent>(_errors, _warnings, content);
        }

        public IApiResponseBuilder WithError(ApiError error)
        {
            if (_errors == null) _errors = new List<ApiError>();
            _errors.Add(error);
            return this;
        }

        public IApiResponseBuilder WithErrors(IEnumerable<ApiError> errors)
        {
            if (_errors == null) _errors = new List<ApiError>();
            _errors.AddRange(errors);
            return this;
        }

        public IApiResponseBuilder WithWarning(ApiWarning warning)
        {
            if (_warnings == null) _warnings = new List<ApiWarning>();
            _warnings.Add(warning);
            return this;
        }

        public IApiResponseBuilder WithWarnings(IEnumerable<ApiWarning> warnings)
        {
            if (_warnings == null) _warnings = new List<ApiWarning>();
            _warnings.AddRange(warnings);
            return this;
        }
    }
}
