namespace IntegrationTests.Helpers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Web application factory for integration tests.
    /// </summary>
    /// <typeparam name="TStartup">The type of the startup class.</typeparam>
    public class TplStatsWebAppFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        private readonly string? connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="TplStatsWebAppFactory{TStartup}"/> class.
        /// </summary>
        /// <param name="connectionString">Database connection string.</param>
        public TplStatsWebAppFactory(string? connectionString = default)
        {
            this.connectionString = connectionString;
        }

        /// <inheritdoc/>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    var dict = new Dictionary<string, string>
                    {
                        { "ConnectionStrings:TplStats", connectionString },
                    };
                    config.AddInMemoryCollection(dict);
                }
            });
        }
    }
}
