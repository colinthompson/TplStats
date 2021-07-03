namespace TplStats.Integrations.Zuluru
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

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
        public static IServiceCollection AddZuluruIntegration(this IServiceCollection services, IConfigurationSection zuluruConfiguration)
        {
            return services;
        }
    }
}
