namespace IntegrationTests.Api.Games.Id
{
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using IntegrationTests.Helpers;
    using NodaTime;
    using TplStats.Core.Entities;
    using Xunit;
    using Xunit.Abstractions;
    using static TplStats.Web.ViewModels;

    /// <summary>
    /// Integration tests for <c>GET</c> requests to <c>/api/games/{id}</c>.
    /// </summary>
    public class GET : IntegrationTestBase
    {
        private const string BaseURI = "/api/games";

        /// <summary>
        /// Initializes a new instance of the <see cref="GET"/> class.
        /// </summary>
        /// <param name="testOutputHelper">Test output helper.</param>
        public GET(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Ensures the response is <c>404 NOT FOUND</c> when no game with the given id exists.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task NotFoundWhenNoSuchTeamExistsAsync()
        {
            // Arrange
            const int id = 42;

            // Act
            using var response = await Client.GetAsync($"{BaseURI}/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// Ensures the response contains the requested game.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsRequestedGame()
        {
            // Arrange
            var start = new LocalDate(2021, 1, 1);
            var end = start + Period.FromMonths(1);
            var season = new Season(1, "test", start, end);
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
            var game = games[2];

            // Act
            using var response = await Client.GetAsync($"{BaseURI}/{game.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadFromJsonAsync<GameModel>(SerializerOptions);
            Assert.Equal(Mapper.Map<GameModel>(game), actual);
        }
    }
}
