namespace UnitTests.Integrations.Zuluru.Services.Helpers
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using TplStats.Integrations.Zuluru;
    using TplStats.Integrations.Zuluru.Options;
    using TplStats.Integrations.Zuluru.Services;
    using Xunit;

    /// <summary>
    /// Base class for unit tests of services that interact with Zuluru servers.
    /// </summary>
    /// <typeparam name="TService">The service type under test.</typeparam>
    /// <typeparam name="TImplementation">The type implementing <typeparamref name="TService"/>.</typeparam>
    public abstract class ZuluruServiceTestBase<TService, TImplementation> : IDisposable
        where TService : notnull
        where TImplementation : ZuluruServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZuluruServiceTestBase{TService, TImplementation}"/> class.
        /// </summary>
        protected ZuluruServiceTestBase()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Testing.json")
                .Build();
            var services = new ServiceCollection();
            services.AddZuluruIntegration(config);

            ServiceProvider = services.BuildServiceProvider();
            Configuration = ServiceProvider.GetRequiredService<IOptions<ZuluruOptions>>().Value;
        }

        /// <summary>
        /// Gets the <see cref="ZuluruOptions"/> that were used to configure the service.
        /// </summary>
        protected ZuluruOptions Configuration { get; }

        /// <summary>
        /// Gets the dependency injection container.
        /// </summary>
        protected ServiceProvider ServiceProvider { get; }

        /// <inheritdoc/>
        public virtual void Dispose() => ServiceProvider.Dispose();

        /// <summary>
        /// Ensures the service's dependencies are registered with the service provider, so that the service can be initialized successfully.
        /// </summary>
        [Fact]
        public void CanInitializeService()
        {
            // Arrange

            // Act
            var service = ServiceProvider.GetRequiredService<TService>();

            // Assert
            Assert.NotNull(service);
            Assert.IsAssignableFrom<TService>(service);
        }
    }
}
