using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Phlank.ApiModeling.Extensions
{
    /// <summary>
    /// Extensions to the <see cref="IServiceCollection"/> interface for
    /// configuring the <see cref="IApiResultBuilder"/> as a part of the service
    /// collection.
    /// <para>
    /// Simple usage:
    /// </para>
    /// <code>
    /// public void ConfigureServices(IServiceCollection services)
    /// {
    ///     ...
    ///     services.ConfigureApiResultBuilder;
    ///     ...
    /// }
    /// </code>
    /// <para>
    /// This will add the IApiResponseBuilder as a transient service to the
    /// service collection.
    /// </para>
    /// </summary>
    public static class ApiModelingServiceCollectionExtensions
    {
        private static ApiModelingOptions _options = new ApiModelingOptions();

        /// <summary>
        /// Adds ApiResponse services to the specified
        /// <see cref="IServiceCollection"/>.
        /// </summary>
        public static void ConfigureApiResultBuilder(this IServiceCollection services, Action<ApiModelingOptions> configureOptions = null)
        {
            var config = configureOptions ?? DefaultConfigureOptions;
            services.Configure(config);

            config(_options);

            services.AddTransient<IApiResultBuilder, ApiResultBuilder>();

            if (_options.UseResponderInvalidModelStateResponseFactory)
            {
                services.PostConfigure<ApiBehaviorOptions>(options =>
                {
                    options.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory;
                });
            }
        }

        private static readonly Action<ApiModelingOptions> DefaultConfigureOptions = (options) =>
        {
            options.UseResponderInvalidModelStateResponseFactory = false;
            options.CharSet = "utf-8";
        };

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

            var options = actionContext.HttpContext.RequestServices.GetRequiredService<IOptions<ApiModelingOptions>>();

            return new ApiResultBuilder(options)
                .WithErrors(apiErrors)
                .Build();
        };
    }
}
