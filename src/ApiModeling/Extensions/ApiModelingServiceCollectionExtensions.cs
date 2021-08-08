using Microsoft.Extensions.DependencyInjection;
using System;

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
    ///     services.ConfigureApiResponseBuilder();
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
        /// <summary>
        /// Adds ApiResponse services to the specified
        /// <see cref="IServiceCollection"/>.
        /// </summary>
        public static void ConfigureApiResponseBuilder(this IServiceCollection services, Action<ApiModelingOptionsBuilder> configureOptions = null)
        {
            services.AddTransient<IApiResultBuilder, ApiResultBuilder>();
            if (configureOptions != null)
            {
                var options = new ApiModelingOptionsBuilder();
                configureOptions(options);
                options.InjectServiceCollection(services).Build();
            }
        }
    }
}
