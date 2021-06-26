namespace UnitTests.Web.Controllers.Helpers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using TplStats.Infrastructure.Database;
    using TplStats.Web;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Base class for unit testing HTTP API controllers.
    /// </summary>
    public abstract class ControllerTestBase : XunitContextBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerTestBase"/> class.
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
            services.AddAutoMapper(typeof(Startup).Assembly);

            Provider = services.BuildServiceProvider();
            Db = Provider.GetRequiredService<TplStatsContext>();
            Mapper = Provider.GetRequiredService<IMapper>();
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        protected TplStatsContext Db { get; }

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        protected IMapper Mapper { get; }

        private ServiceProvider Provider { get; }

        /// <inheritdoc/>
        public override void Dispose()
        {
            Provider.Dispose();
            base.Dispose();
        }
    }
}
