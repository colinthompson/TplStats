namespace IntegrationTests.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using NodaTime;
    using NodaTime.Serialization.SystemTextJson;
    using TplStats.Infrastructure.Database;
    using TplStats.Web;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Base class for integration tests.
    /// </summary>
    public abstract class IntegrationTestBase : XunitContextBase, IAsyncLifetime
    {
        private readonly IServiceScope scope;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationTestBase"/> class.
        /// </summary>
        /// <param name="testOutputHelper">Test output helper.</param>
        protected IntegrationTestBase(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            var connStr = FormatConnectionString(
                string.Format(
                    "{0}_{1}",
                    Context.UniqueTestName,
                    Guid.NewGuid()));
            Factory = new TplStatsWebAppFactory<Startup>(connStr);
            Client = Factory.CreateClient();

            scope = Factory.Services.CreateScope();
            SerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web).ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            Mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        }

        /// <summary>
        /// Gets the web application factory.
        /// </summary>
        protected TplStatsWebAppFactory<Startup> Factory { get; }

        /// <summary>
        /// Gets the http client.
        /// </summary>
        protected HttpClient Client { get; }

        /// <summary>
        /// Gets the JSON Serializer options.
        /// </summary>
        protected JsonSerializerOptions SerializerOptions { get; }

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        protected IMapper Mapper { get; }

        /// <inheritdoc/>
        public override void Dispose()
        {
            GC.SuppressFinalize(this);

            Client.Dispose();
            scope.Dispose();
            Factory.Dispose();
            base.Dispose();
        }

        /// <inheritdoc/>
        public async Task InitializeAsync()
        {
            using var scope = Factory.Services.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<TplStatsContext>();
            await db.Database.EnsureCreatedAsync();
        }

        /// <inheritdoc/>
        public async Task DisposeAsync()
        {
            using var scope = Factory.Services.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<TplStatsContext>();
            await db.Database.EnsureDeletedAsync();
        }

        /// <summary>
        /// Seeds the database with the provided values.
        /// </summary>
        /// <param name="entities">The entites to add to the database.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task SeedDbAsync(IEnumerable<object> entities)
        {
            using var scope = Factory.Services.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<TplStatsContext>();

            db.AddRange(entities);
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Seeds the database with the provided values.
        /// </summary>
        /// <param name="entities">The entites to add to the database.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected Task SeedDbAsync(params object[] entities) => SeedDbAsync(entities.AsEnumerable());

        private static string FormatConnectionString(string dbName)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var port = int.Parse(Environment.GetEnvironmentVariable("DB_PORT") ?? "5432");
            var user = Environment.GetEnvironmentVariable("DB_USER") ?? "tplstats";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "P@ssword";

            return $"Host={host};Port={port};User ID={user};Password={password};Database={dbName};Include Error Detail=true";
        }
    }
}
