namespace TplStats.Infrastructure.Database
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the services necessary for TPL Stat's database access.
        /// </summary>
        /// <param name="services">The dependency injection container.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <returns>
        /// <paramref name="services"/>, to allow for chained method calls.
        /// </returns>
        public static IServiceCollection AddTplStatsDatabase(
            this IServiceCollection services,
            string connectionString)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (connectionString is null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            services.AddDbContext<TplStatsContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.UseNodaTime();
                });
                options.UseSnakeCaseNamingConvention();
            });

            return services;
        }
    }
}
