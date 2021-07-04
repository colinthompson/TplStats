namespace TplStats.Integrations.Zuluru
{
    using Flurl.Http.Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TplStats.Integrations.Zuluru.Interfaces;
    using TplStats.Integrations.Zuluru.Options;
    using TplStats.Integrations.Zuluru.Services;

    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the services necessary to add TPL Stat's Zuluru integration.
        /// </summary>
        /// <param name="services">The dependency injection container.</param>
        /// <param name="zuluruConfiguration">Configuration options for the Zuluru integration.</param>
        /// <returns><paramref name="services"/>, to allow for chained method calls.</returns>
        public static IServiceCollection AddZuluruIntegration(this IServiceCollection services, IConfiguration zuluruConfiguration)
        {
            services.AddMemoryCache();
            services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();

            services.Configure<ZuluruOptions>(zuluruConfiguration);
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}
