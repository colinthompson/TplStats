namespace UnitTests.Web.Controllers.GamesControllerTests
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using NodaTime;
    using TplStats.Core.Entities;
    using TplStats.Web.Controllers;
    using UnitTests.Web.Controllers.Helpers;
    using Xunit;
    using Xunit.Abstractions;
    using static TplStats.Web.ViewModels;

    /// <summary>
    /// Unit tests for <see cref="GamesController.DetailsAsync(int, CancellationToken)"/>.
    /// </summary>
    public class DetailsAsync : ControllerTestBase<GamesController>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DetailsAsync"/> class.
        /// </summary>
        /// <param name="testOutputHelper">Test output helper.</param>
        public DetailsAsync(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Ensures the correct <see cref="Game"/> entity is retrieved.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsCorrectGameAsync()
        {
            // Arrange
            var start = new LocalDate(2021, 1, 1);
            var end = new LocalDate(2021, 1, 31);
            var season = new Season(1, "test", start, end);
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
            var game = games[2];

            // Act
            var result = await Controller.DetailsAsync(game.Id, default);

            // Assert
            Assert.Equal(Mapper.Map<GameModel>(game), result.Value);
        }

        /// <summary>
        /// Ensures a <see cref="NotFoundResult"/> is returned when no <see cref="Game"/> with the specified id exists.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsNotFoundWhenNoSuchGameExistsAsync()
        {
            // Arrange
            const int id = 42;

            // Act
            var result = await Controller.DetailsAsync(id, default);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        /// <summary>
        /// Ensures a request can be cancelled.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public Task RequestCanBeCancelledAsync()
        {
            // Arrange
            using var tokenSource = new CancellationTokenSource();
            tokenSource.Cancel();

            // Act

            // Assert
            return Assert.ThrowsAsync<OperationCanceledException>(() => Controller.DetailsAsync(42, tokenSource.Token));
        }
    }
}
