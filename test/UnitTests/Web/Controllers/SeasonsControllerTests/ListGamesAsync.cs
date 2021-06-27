namespace UnitTests.Web.Controllers.SeasonsControllerTests
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Mvc;
    using NodaTime;
    using TplStats.Core.Entities;
    using TplStats.Web.Controllers;
    using UnitTests.Web.Controllers.Helpers;
    using Xunit;
    using Xunit.Abstractions;
    using static TplStats.Web.ViewModels;

    /// <summary>
    /// Unit tests for <see cref="SeasonsController.ListGamesAsync(int, CancellationToken)"/>.
    /// </summary>
    public class ListGamesAsync : ControllerTestBase<SeasonsController>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListGamesAsync"/> class.
        /// </summary>
        /// <param name="testOutputHelper">Test output helper.</param>
        public ListGamesAsync(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Returns a <see cref="NotFoundResult"/> when the given season does not exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task NotFoundWhenNoSuchSeasonExistsAsync()
        {
            // Arrange
            const int id = 42;

            // Act
            var result = await Controller.ListGamesAsync(id, default);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        /// <summary>
        /// Returns an empty list when no games have been scheduled for the given season.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsEmptyListWhenNoGamesHaveBeenScheduledAsync()
        {
            // Arrange
            var start = new LocalDate(2021, 1, 1);
            var end = new LocalDate(2021, 1, 31);
            var season = new Season(1, "Test", start, end);
            await SeedDbAsync(season);

            // Act
            var result = await Controller.ListGamesAsync(season.Id, default);

            // Assert
            Assert.Empty(result.Value);
        }

        /// <summary>
        /// Returns a list of all games scheduled as part of the given season.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsListOfAllScheduledGamesAsync()
        {
            // Arrange
            var start = new LocalDate(2021, 1, 1);
            var end = new LocalDate(2021, 1, 31);
            var season = new Season(1, "Test", start, end);
            var homeTeam = season.AddTeam(1, "Home Team");
            var awayTeam = season.AddTeam(2, "Away Team");
            var games = Enumerable.Range(1, 5)
                .Select(x =>
                {
                    var start = season.StartDate.AtMidnight() + Period.FromHours(x);
                    var end = start + Period.FromHours(1);
                    var field = $"Field #{x}";

                    return season.ScheduleGame(x, start, end, field, homeTeam, awayTeam);
                })
                .ToList();
            await SeedDbAsync(season);

            // Act
            var result = await Controller.ListGamesAsync(season.Id, default);

            // Assert
            Assert.Equal(games.AsQueryable().ProjectTo<GameModel>(Mapper.ConfigurationProvider), result.Value);
        }
    }
}
