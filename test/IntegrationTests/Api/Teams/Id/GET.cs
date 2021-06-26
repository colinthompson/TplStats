namespace IntegrationTests.Api.Teams.Id
{
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using IntegrationTests.Helpers;
    using TplStats.Core.Entities;
    using Xunit;
    using Xunit.Abstractions;
    using static TplStats.Web.ViewModels;

    /// <summary>
    /// Integration tests for <c>GET</c> requests to <c>/api/teams/{id}</c>.
    /// </summary>
    public class GET : IntegrationTestBase
    {
        private const string BaseURI = "/api/teams";

        /// <summary>
        /// Initializes a new instance of the <see cref="GET"/> class.
        /// </summary>
        /// <param name="testOutputHelper">Test output helper.</param>
        public GET(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Ensures the response is <c>404 NOT FOUND</c> when no team with the given id exists.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task NotFoundWhenNoSuchTeamExistsAsync()
        {
            // Arrange
            var season = new Season(1, "test", default, default);
            var teams = Enumerable.Range(1, 10)
                .Select(x => season.AddTeam(x, $"{x}"))
                .ToList();
            await SeedDbAsync(season);
            const int id = 42;

            // Act
            using var response = await Client.GetAsync($"{BaseURI}/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// Ensures the response contains the requested team.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsRequestedTeam()
        {
            // Arrange
            var season = new Season(1, "test", default, default);
            var teams = Enumerable.Range(1, 10)
                .Select(x => season.AddTeam(x, $"{x}"))
                .ToList();
            await SeedDbAsync(season);
            var team = teams[5];

            // Act
            using var response = await Client.GetAsync($"{BaseURI}/{team.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadFromJsonAsync<TeamModel>(SerializerOptions);
            Assert.Equal(Mapper.Map<TeamModel>(team), actual);
        }
    }
}
