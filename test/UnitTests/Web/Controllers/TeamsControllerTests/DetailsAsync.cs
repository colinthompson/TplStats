namespace UnitTests.Web.Controllers.TeamsControllerTests
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
    using static TplStats.Web.ViewModels;

    /// <summary>
    /// Unit tests for <see cref="TeamsController.DetailsAsync(int, CancellationToken)"/>.
    /// </summary>
    public class DetailsAsync : ControllerTestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DetailsAsync"/> class.
        /// </summary>
        /// <param name="testOutputHelper">Test output helper.</param>
        public DetailsAsync(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            Controller = new(Db, Mapper);
        }

        private TeamsController Controller { get; }

        /// <summary>
        /// Ensures the correct <see cref="Team"/> entity is retrieved.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsCorrectTeamAsync()
        {
            // Arrange
            var season = new Season(1, "test", default, default);
            var teams = Enumerable.Range(1, 10)
                .Select(x => season.AddTeam(x, $"Team #{x}"))
                .ToList();
            Db.AddRange(teams);
            await Db.SaveChangesAsync();
            var team = teams[5];

            // Act
            var result = await Controller.DetailsAsync(team.Id, default);

            // Assert
            Assert.Equal(Mapper.Map<TeamModel>(team), result.Value);
        }

        /// <summary>
        /// Ensures a <see cref="NotFoundResult"/> is returned when no <see cref="Team"/> with the specified id exists.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsNotFoundWhenNoSuchTeamExistsAsync()
        {
            // Arrange
            var season = new Season(1, "test", default, default);
            var teams = Enumerable.Range(1, 10)
                .Select(x => season.AddTeam(x, $"Team #{x}"))
                .ToList();
            Db.AddRange(teams);
            await Db.SaveChangesAsync();
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
