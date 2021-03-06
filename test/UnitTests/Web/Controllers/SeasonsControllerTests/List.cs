namespace UnitTests.Web.Controllers.SeasonsControllerTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using TplStats.Core.Entities;
    using TplStats.Web.Controllers;
    using UnitTests.Web.Controllers.Helpers;
    using Xunit;
    using Xunit.Abstractions;
    using static TplStats.Web.ViewModels;

    /// <summary>
    /// Unit tests for <see cref="SeasonsController.List"/>.
    /// </summary>
    public class List : ControllerTestBase<SeasonsController>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="List"/> class.
        /// </summary>
        /// <param name="testOutputHelper">Test output helper.</param>
        public List(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

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
            await SeedDbAsync(seasons);

            // Act
            var result = await Controller.List().ToListAsync();

            // Assert
            Assert.Equal(seasons.AsQueryable().ProjectTo<SeasonModel>(Mapper.ConfigurationProvider), result);
        }
    }
}
