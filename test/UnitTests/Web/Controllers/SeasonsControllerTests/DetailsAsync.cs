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
    /// Unit tests for <see cref="SeasonsController.DetailsAsync(int, CancellationToken)"/>.
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
            Controller = new(Db);
        }

        private SeasonsController Controller { get; }

        /// <summary>
        /// Ensures the correct <see cref="Season"/> entity is retrieved.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsCorrectSeasonAsync()
        {
            // Arrange
            var seasons = Enumerable.Range(1, 10)
                .Select(x => new Season(x, $"Season #{x}", default, default))
                .ToList();
            Db.AddRange(seasons);
            await Db.SaveChangesAsync();
            var season = seasons[5];

            // Act
            var result = await Controller.DetailsAsync(season.Id, default);

            // Assert
            Assert.Equal(season, result.Value);
        }

        /// <summary>
        /// Ensures a <see cref="NotFoundResult"/> is returned when no <see cref="Season"/> with the specified id exists.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsNotFoundWhenNoSuchSeasonExistsAsync()
        {
            // Arrange
            var seasons = Enumerable.Range(1, 10)
                .Select(x => new Season(x, $"Season #{x}", default, default))
                .ToList();
            Db.AddRange(seasons);
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
