namespace UnitTests.Web.Controllers.SeasonsControllerTests
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using TplStats.Core.Entities;
    using TplStats.Web.Controllers;
    using UnitTests.Web.Controllers.Helpers;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Unit tests for <see cref="SeasonsController.ListTeamsAsync(int, System.Threading.CancellationToken)"/>.
    /// </summary>
    public class ListTeamsAsync : ControllerTestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListTeamsAsync"/> class.
        /// </summary>
        /// <param name="testOutputHelper">Test output helper.</param>
        public ListTeamsAsync(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            Controller = new(Db, Mapper);
        }

        private SeasonsController Controller { get; }

        /// <summary>
        /// Ensures the ids for all <see cref="Team"/>s competing in the season are returned.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsListOfTeamIdsAsync()
        {
            // Arrange
            var season = new Season(42, "Test Season", default, default);
            var teams = Enumerable.Range(1, 10)
                .Select(x => season.AddTeam(x, $"#{x}"))
                .ToList();
            Db.Add(season);
            await Db.SaveChangesAsync();

            // Act
            var result = await Controller.ListTeamsAsync(season.Id, default);

            // Assert
            Assert.Equal(teams.Select(t => t.Id), result.Value);
        }

        /// <summary>
        /// Ensures a <see cref="NotFoundResult"/> is returned when now <see cref="Season"/> with the specified id exists.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsNotFoundWhenNoSuchSeasonExistsAsync()
        {
            // Arrange
            var seasons = Enumerable.Range(1, 10)
                .Select(x => new Season(x, $"Season ${x}", default, default))
                .ToList();
            Db.AddRange(seasons);
            await Db.SaveChangesAsync();
            const int id = 42;

            // Act
            var result = await Controller.ListTeamsAsync(id, default);

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
            return Assert.ThrowsAsync<OperationCanceledException>(() => Controller.ListTeamsAsync(42, tokenSource.Token));
        }
    }
}
