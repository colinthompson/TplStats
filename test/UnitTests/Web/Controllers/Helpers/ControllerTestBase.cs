namespace UnitTests.Web.Controllers.Helpers
{
    using Microsoft.EntityFrameworkCore;
    using TplStats.Infrastructure.Database;
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
            var options = new DbContextOptionsBuilder<TplStatsContext>()
                .UseInMemoryDatabase(Context.UniqueTestName)
                .Options;
            Db = new(options);
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        protected TplStatsContext Db { get; }
    }
}
