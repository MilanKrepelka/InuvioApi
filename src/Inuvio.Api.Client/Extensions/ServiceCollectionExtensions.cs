using ASOL.Inuvio.Api.Client.Contracts;
using ASOL.Inuvio.Api.Client.Handlers;
using ASOL.Inuvio.Api.Client.Options;
using ASOL.Inuvio.Api.Client.SystemApi;
using ASOL.Inuvio.Api.Client.Wrappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ASOL.Inuvio.Api.Client.Extensions
{
    /// <summary>
    /// ServiceCollectionExtensions for registering the iNuvio API client and its dependencies into the .NET dependency injection container. This class provides an extension method to simplify the setup of the iNuvio API client, including configuration options, token providers, HTTP handlers, and Refit clients for making API calls. By using this extension method, developers can easily integrate the iNuvio API client into their applications with a clean and maintainable approach.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        
        private static IServiceCollection addAddInuvioApiClientServices(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeNowWrapper, DateTimeNowWrapper>();
            services.AddTransient<IInuvioApiTokenGenerator, InuvioApiTokenGenerator>();

            // Token provider (Singleton for caching)
            services.AddSingleton<IInuvioApiTokenProvider, InuvioOauth2ApiTokenProvider>();

            // Handlers
            services.AddTransient<InuvioAuthHandler>();
            services.AddTransient<TimingDelegatingHandler>();

            // Refit settings with System.Text.Json
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        Converters = { new JsonStringEnumConverter() }
                    })
            };

            // Register INTERNAL Refit client
            services.AddRefitClient<IInuvioSystemApiRefit>(refitSettings)
                .ConfigureHttpClient((sp, client) =>
                {

                    var options = sp.GetRequiredService<IOptions<InuvioApiConnectionOptions>>().Value;
                    client.BaseAddress = new Uri(options.ServerUrlWithoutTrailingSlash);
                    client.Timeout = TimeSpan.FromSeconds(30);
                })
                .AddHttpMessageHandler<TimingDelegatingHandler>()
                .AddHttpMessageHandler<InuvioAuthHandler>();

            // Register PUBLIC contract implementation (Adapter)
            services.AddTransient<IInuvioSystemApi, InuvioSystemApi>();

            // Main client facade
            services.AddTransient<IInuvioApiClient, InuvioApiClient>();

            // Named HttpClient for OAuth (used by token provider)
            services.AddHttpClient("iNuvioAPIAuth", (sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<InuvioApiConnectionOptions>>().Value;
                client.BaseAddress = new Uri(options.ServerUrlWithoutTrailingSlash);
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            return services;
        }
        public static IServiceCollection AddInuvioApiClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configuration options
            services.AddOptions<InuvioApiConnectionOptions>();
            services.Configure<InuvioApiConnectionOptions>(
                configuration.GetSection("InuvioApiConnection"));

            return addAddInuvioApiClientServices(services);
        }

       

        public static IServiceCollection AddInuvioApiClient(
            this IServiceCollection services,
            Action<InuvioApiConnectionOptions> configureOptions)
        {
            services.AddOptions<InuvioApiConnectionOptions>();
            services.Configure(configureOptions);

            return addAddInuvioApiClientServices(services);
        }
    }
}
