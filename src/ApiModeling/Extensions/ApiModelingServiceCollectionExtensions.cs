using Microsoft.Extensions.DependencyInjection;
using System;

namespace Phlank.ApiModeling.Extensions
{
    public static class ApiModelingServiceCollectionExtensions
    {
        /// <summary>
        /// Adds ApiResponse services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        public static void ConfigureApiResponseBuilder(this IServiceCollection services, Action<ApiModelingOptionsBuilder> configureOptions = null)
        {
            services.AddTransient<IApiResponseBuilder, ApiResponseBuilder>();
            if (configureOptions != null)
            {
                var options = new ApiModelingOptionsBuilder();
                configureOptions(options);
                options.InjectServiceCollection(services).Build();
            }
        }
    }
}
