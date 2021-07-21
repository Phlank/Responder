using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Phlank.ApiModeling
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiModelingOptionsBuilder
    {
        private IServiceCollection _services;
        private bool _isUsingApiResponseForModelStateErrors = false;

        /// <summary>
        /// Overrides the configured InvalidModelStateResponseFactory to use the <see cref="ApiResponse"/>.
        /// </summary>
        public ApiModelingOptionsBuilder UseApiResponseForModelStateErrors()
        {
            _isUsingApiResponseForModelStateErrors = true;
            return this;
        }

        internal ApiModelingOptionsBuilder InjectServiceCollection(IServiceCollection services)
        {
            _services = services;
            return this;
        }

        internal void Build()
        {
            var provider = _services.BuildServiceProvider();
            if (_isUsingApiResponseForModelStateErrors) ConfigureApiBehaviorOptions(provider);
        }

        private void ConfigureApiBehaviorOptions(ServiceProvider provider)
        {
            var apiBehaviorOptions = provider.GetService<ApiBehaviorOptions>();
            if (apiBehaviorOptions == null)
            {
                _services.Configure<ApiBehaviorOptions>(options => { options.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory; });
            }
            else
            {
                apiBehaviorOptions.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory;
                _services.Configure<ApiBehaviorOptions>(options => { options = apiBehaviorOptions; });
            }
        }

        private static Func<ActionContext, IActionResult> InvalidModelStateResponseFactory = actionContext =>
            new ContentResult
            {
                Content = JsonSerializer.Serialize(ConvertModelStateDictionaryToApiResponse(actionContext.ModelState)),
                ContentType = "application/json"
            };

        private static ApiResponse ConvertModelStateDictionaryToApiResponse(ModelStateDictionary dictionary)
        {
            var invalidKeys = dictionary.Keys.Where(key =>
                dictionary.GetValueOrDefault(key) != default
                && dictionary.GetValueOrDefault(key).ValidationState == ModelValidationState.Invalid);

            var apiErrors = invalidKeys.SelectMany(key => dictionary.GetValueOrDefault(key).Errors.Select(error => new ApiError()
            {
                Code = error.Exception?.GetType().Name ?? "InvalidField",
                Fields = new List<string> { key },
                Message = error.ErrorMessage
            }));

            return new ApiResponseBuilder()
                .WithErrors(apiErrors)
                .Build();
        }
    }
}