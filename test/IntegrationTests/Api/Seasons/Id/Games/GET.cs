namespace IntegrationTests.Api.Seasons.Id.Games
{
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using IntegrationTests.Helpers;
    using NodaTime;
    using TplStats.Core.Entities;
    using Xunit;
    using Xunit.Abstractions;
    using static TplStats.Web.ViewModels;

    /// <summary>
    /// Integration test for <c>GET</c> requests to <c>/api/seasons/{id}}/games</c>.
    /// </summary>
    public class GET : IntegrationTestBase
    {
        private const string BaseUri = "/api/seasons";

        /// <summary>
        /// Initializes a new instance of the <see cref="GET"/> class.
        /// </summary>
        /// <param name="testOutputHelper">Test output helper.</param>
        public GET(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Ensures the response is <c>404 NOT FOUND</c> when no season with the given id eixsts.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task NotFoundWhenNoSuchSeasonExistsAsync()
        {
            // Arrange
            const int id = 42;

            // Act
            using var response = await Client.GetAsync($"{BaseUri}/{id}/games");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// Ensures an empty list is returned when the requested season has no games scheduled.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task EmptyListWhenNoGamesHaveBeenScheduledAsync()
        {
            // Arrange
            var startDate = new LocalDate(2021, 1, 1);
            var endDate = startDate + Period.FromMonths(1);
            var season = new Season(1, "Test season", startDate, endDate);
            await SeedDbAsync(season);

            // Act
            using var response = await Client.GetAsync($"{BaseUri}/{season.Id}/games");

            // Assert
            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadFromJsonAsync<GameModel[]>(SerializerOptions);
            Assert.Empty(actual);
        }

        /// <summary>
        /// Returns a list of all the games that have been scheduled as part of the requested season.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsListOfScheduledGamesAsync()
        {
            // Arrange
            var startDate = new LocalDate(2021, 1, 1);
            var endDate = startDate + Period.FromMonths(1);
            var season = new Season(1, "Test season", startDate, endDate);
            var homeTeam = season.AddTeam(1, "Home team");
            var awayTeam = season.AddTeam(2, "Away team");
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
            using var response = await Client.GetAsync($"{BaseUri}/{season.Id}/games");

            // Assert
            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadFromJsonAsync<GameModel[]>(SerializerOptions);
            Assert.Equal(games.AsQueryable().ProjectTo<GameModel>(Mapper.ConfigurationProvider), actual);
        }
    }
}
