using ASOL.Inuvio.Api.Client.AVAModelsApi;
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
        
        /// <summary>
        /// Adds and configures all required services for the Inuvio API client, including authentication, token
        /// management, HTTP handlers, and Refit clients, to the specified service collection.
        /// </summary>
        /// <remarks>This method registers all dependencies needed for consuming the Inuvio API, including
        /// token providers, HTTP handlers, and Refit-based API clients. It should be called during application startup
        /// as part of the dependency injection configuration.</remarks>
        /// <param name="services">The service collection to which the Inuvio API client services will be added. Must not be null.</param>
        /// <returns>The same service collection instance with the Inuvio API client services registered.</returns>
        private static IServiceCollection AddAddInuvioApiClientServices(this IServiceCollection services)
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
                        Converters = { new JsonStringEnumConverter() },
                        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
                    })
            };

            // Register INTERNAL Refit clients
            AddRefitClientWithAuth<IInuvioSystemApiRefit>(services, refitSettings);
            AddRefitClientWithAuth<IAVAModelsApiRefit>(services, refitSettings);

            // Register PUBLIC contract implementation (Adapter)
            services.AddTransient<IInuvioSystemApi, InuvioSystemApi>();
            services.AddTransient<IAVAModelsApi, AVAModelsApi.InuvioAVAModelsApi>();

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

        /// <summary>
        /// Adds a Refit client of the specified interface type to the service collection and configures it with
        /// authentication and custom HTTP handlers.
        /// </summary>
        /// <remarks>This method configures the Refit client with a base address and timeout, and adds
        /// HTTP message handlers for timing and authentication. The client will use the server URL specified in the
        /// registered InuvioApiConnectionOptions. This method is intended for use during application startup when
        /// registering services for dependency injection.</remarks>
        /// <typeparam name="T">The interface type of the Refit client to register. Must be a class.</typeparam>
        /// <param name="services">The service collection to which the Refit client will be added.</param>
        /// <param name="refitSettings">The Refit settings to use when configuring the client.</param>
        private static void AddRefitClientWithAuth<T>(IServiceCollection services, RefitSettings refitSettings) where T : class
        {
            services.AddRefitClient<T>(refitSettings)
                .ConfigureHttpClient((sp, client) =>
                {
                    var options = sp.GetRequiredService<IOptions<InuvioApiConnectionOptions>>().Value;
                    client.BaseAddress = new Uri(options.ServerUrlWithoutTrailingSlash);
                    client.Timeout = TimeSpan.FromSeconds(30);
                })
                .AddHttpMessageHandler<TimingDelegatingHandler>()
                .AddHttpMessageHandler<InuvioAuthHandler>();
        }

        /// <summary>
        /// Adds and configures the Inuvio API client and its related services to the specified service collection.
        /// </summary>
        /// <remarks>This method registers the Inuvio API client and binds configuration settings from the
        /// 'InuvioApiConnection' section. Call this method during application startup to enable API client
        /// functionality throughout the application.</remarks>
        /// <param name="services">The service collection to which the Inuvio API client services will be added. Must not be null.</param>
        /// <param name="configuration">The application configuration containing the Inuvio API connection settings. Must not be null.</param>
        /// <returns>The service collection with the Inuvio API client services registered. This enables dependency injection of
        /// the API client and related options.</returns>
        public static IServiceCollection AddInuvioApiClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configuration options
            services.AddOptions<InuvioApiConnectionOptions>();
            services.Configure<InuvioApiConnectionOptions>(
                configuration.GetSection("InuvioApiConnection"));

            return AddAddInuvioApiClientServices(services);
        }
        /// <summary>
        /// Adds and configures the Inuvio API client and its dependencies to the specified service collection.
        /// </summary>
        /// <remarks>Call this method during application startup to register the Inuvio API client for
        /// dependency injection. The provided configuration delegate allows customization of connection
        /// settings.</remarks>
        /// <param name="services">The service collection to which the Inuvio API client services will be added.</param>
        /// <param name="configureOptions">A delegate used to configure the options for connecting to the Inuvio API.</param>
        /// <returns>The service collection with the Inuvio API client services registered. This enables chaining additional
        /// service configuration calls.</returns>
        public static IServiceCollection AddInuvioApiClient(
            this IServiceCollection services,
            Action<InuvioApiConnectionOptions> configureOptions)
        {
            services.AddOptions<InuvioApiConnectionOptions>();
            services.Configure(configureOptions);

            return AddAddInuvioApiClientServices(services);
        }
    }
}
