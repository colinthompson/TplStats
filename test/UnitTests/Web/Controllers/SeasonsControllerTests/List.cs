namespace UnitTests.Web.Controllers.SeasonsControllerTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using TplStats.Core.Entities;
    using TplStats.Web.Controllers;
    using UnitTests.Web.Controllers.Helpers;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Unit tests for <see cref="SeasonsController.List"/>.
    /// </summary>
    public class List : ControllerTestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="List"/> class.
        /// </summary>
        /// <param name="testOutputHelper">Test output helper.</param>
        public List(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            Controller = new(Db);
        }

        private SeasonsController Controller { get; }

        /// <summary>
        /// Ensures an empty list is returned when no <see cref="Season"/> entities exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task EmptyListWhenNoSeasonsExistAsync()
        {
            // Arrange

            // Act
            var result = await Controller.List().ToListAsync();

            // Assert
            Assert.Empty(result);
        }

        /// <summary>
        /// Ensurse a list containing all known <see cref="Season"/> entities is returned.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task RetrievesAllSeasonsAsync()
        {
            // Arrange
            var seasons = Enumerable.Range(1, 10)
                .Select(x => new Season(x, $"Season #{x}", default, default))
                .ToList();
            Db.AddRange(seasons);
            await Db.SaveChangesAsync();

            // Act
            var result = await Controller.List().ToListAsync();

            // Assert
            Assert.Equal(seasons, result);
        }
    }
}
