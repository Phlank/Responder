using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace Phlank.ApiModeling
{
    /// <summary>
    /// </summary>
    public class ApiModelingOptionsBuilder
    {
        private IServiceCollection _services;
        private bool _isUsingApiResponseForModelStateErrors = false;

        /// <summary>
        /// Overrides the configured InvalidModelStateResponseFactory to use the
        /// <see cref="ApiResponse"/>.
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
            if (_isUsingApiResponseForModelStateErrors) ConfigureApiBehaviorOptions();
        }

        private void ConfigureApiBehaviorOptions()
        {
            _services.PostConfigure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory;
            });
        }

        private static readonly Func<ActionContext, IActionResult> InvalidModelStateResponseFactory = actionContext =>
        {
            var modelState = actionContext.ModelState;

            var invalidKeys = modelState.Keys.Where(key =>
                modelState.GetValueOrDefault(key)?.ValidationState == ModelValidationState.Invalid);

            var apiErrors = invalidKeys.SelectMany(key => modelState.GetValueOrDefault(key).Errors.Select(error => new ApiError()
            {
                Detail = error.ErrorMessage,
                Title = "The content provided did not match the pattern expected",
                Status = HttpStatusCode.BadRequest,
                Extensions = new Dictionary<string, object>
                {
                    { "field", key },
                    { "trace", actionContext.HttpContext.TraceIdentifier }
                }
            }));

            return new ApiResultBuilder()
                .WithErrors(apiErrors)
                .Build();
        };
    }
}