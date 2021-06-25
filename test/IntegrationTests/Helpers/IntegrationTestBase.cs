namespace IntegrationTests.Helpers
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
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
            var connStr = FormatConnectionString(Context.UniqueTestName);
            Factory = new TplStatsWebAppFactory<Startup>(connStr);
            Client = Factory.CreateClient();

            scope = Factory.Services.CreateScope();
            Db = scope.ServiceProvider.GetRequiredService<TplStatsContext>();
        }

        /// <summary>
        /// Gets the web application factory.
        /// </summary>
        protected TplStatsWebAppFactory<Startup> Factory { get; }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        protected TplStatsContext Db { get; }

        /// <summary>
        /// Gets the http client.
        /// </summary>
        protected HttpClient Client { get; }

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
        public Task InitializeAsync() => Db.Database.EnsureCreatedAsync();

        /// <inheritdoc/>
        public Task DisposeAsync() => Db.Database.EnsureDeletedAsync();

        private static string FormatConnectionString(string dbName)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var port = int.Parse(Environment.GetEnvironmentVariable("DB_PORT") ?? "5432");
            var user = Environment.GetEnvironmentVariable("DB_USER") ?? "tpl";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "P@ssword";

            return $"Host={host};Port={port};User ID={user};Password={password};Database={dbName}";
        }
    }
}
