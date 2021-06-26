namespace UnitTests.Web.Controllers.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using TplStats.Infrastructure.Database;
    using TplStats.Web;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Base class for unit testing HTTP API controllers.
    /// </summary>
    /// <typeparam name="TController">API controller type under test.</typeparam>
    public abstract class ControllerTestBase<TController> : XunitContextBase
        where TController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerTestBase{T}"/> class.
        /// </summary>
        /// <param name="testOutputHelper">Test output helper.</param>
        protected ControllerTestBase(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            var services = new ServiceCollection();
            services.AddDbContext<TplStatsContext>(opts =>
            {
                opts.UseInMemoryDatabase(Context.UniqueTestName);
            });
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup).Assembly);

            // Add controllers. Bit of a hack, but saves us from having to create separate base classes for each controller type.
            var controllerTypes = typeof(Startup).Assembly.ExportedTypes
                .Where(type => type.IsSubclassOf(typeof(ControllerBase)))
                .ToArray();
            foreach (var controllerType in controllerTypes)
            {
                services.AddScoped(controllerType);
            }

            Provider = services.BuildServiceProvider();
            Mapper = Provider.GetRequiredService<IMapper>();
            Controller = Provider.GetRequiredService<TController>();
        }

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        protected IMapper Mapper { get; }

        /// <summary>
        /// Gets the HTTP API controller.
        /// </summary>
        protected TController Controller { get; }

        private ServiceProvider Provider { get; }

        /// <inheritdoc/>
        public override void Dispose()
        {
            Provider.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Seeds the database with the provided values.
        /// </summary>
        /// <param name="entities">The entites to add to the database.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task SeedDbAsync(IEnumerable<object> entities)
        {
            using var scope = Provider.CreateScope();
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
    }
}
