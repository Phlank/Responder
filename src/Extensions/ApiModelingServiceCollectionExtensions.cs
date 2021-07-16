using Microsoft.Extensions.DependencyInjection;
using System;

namespace Phlank.ApiModeling.Extensions
{
    public static class ApiModelingServiceCollectionExtensions
    {
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
