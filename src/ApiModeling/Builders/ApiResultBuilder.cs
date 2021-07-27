using Microsoft.AspNetCore.Mvc;
using Phlank.ApiModeling.Extensions;
using RestSharp;
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
            object resultValue;
            int resultStatus;
            string resultContentType;

            if (_errors.Count > 0)
            {
                resultValue = CreateErrorValue();
                resultStatus = (int)_errors.First().Status;
                resultContentType = "application/problem+json";
            } else
            {
                resultValue = CreateSuccessValue();
                resultStatus = (int)_successStatusCode;
                resultContentType = "application/json";
            }

            var result = new ApiResult(resultValue);
            result.StatusCode = resultStatus;
            result.ContentType = resultContentType;

            return result;
        }

        private ApiError CreateErrorValue()
        {
            var firstError = _errors.First();
            var remainingErrors = _errors.GetRange(1, _errors.Count - 1);

            var extensions = firstError.Extensions;
            extensions.Add("otherErrors", remainingErrors);

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

        public IApiResultBuilder WithError(ApiError error)
        {
            _errors.Add(error);
            return this;
        }

        public IApiResultBuilder WithErrors(IEnumerable<ApiError> errors)
        {
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
