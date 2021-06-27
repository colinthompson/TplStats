namespace IntegrationTests.Api.Seasons.Id.Teams
{
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using FluentAssertions;
    using IntegrationTests.Helpers;
    using TplStats.Core.Entities;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Integration tests for <c>GET</c> requests to <c>/api/seasons/{id}/teams</c>.
    /// </summary>
    public class GET : IntegrationTestBase
    {
        private const string BaseURI = "/api/seasons";

        /// <summary>
        /// Initializes a new instance of the <see cref="GET"/> class.
        /// </summary>
        /// <param name="testOutputHelper">Test output helper.</param>
        public GET(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Ensures the response is <c>404 NOT FOUND</c> when no season with the given id exists.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task NotFoundWhenNoSuchSeasonExistsAsync()
        {
            // Arrange
            var seasons = Enumerable.Range(1, 10)
                .Select(x => new Season(x, $"{x}", default, default))
                .ToList();
            await SeedDbAsync(seasons);
            const int id = 42;

            // Act
            using var response = await Client.GetAsync($"{BaseURI}/{id}/teams");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// Ensures an empty list is returned when the requested season has no teams.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task EmptyListWhenNoTeamsExistAsync()
        {
            // Arrange
            var seasons = Enumerable.Range(1, 10)
                .Select(x => new Season(x, $"{x}", default, default))
                .ToList();
            await SeedDbAsync(seasons);
            var season = seasons[5];

            // Act
            using var response = await Client.GetAsync($"{BaseURI}/{season.Id}/teams");

            // Assert
            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadFromJsonAsync<int[]>(SerializerOptions);
            Assert.Empty(actual);
        }

        /// <summary>
        /// Returns a list of the ids of all teams competing in the requested season.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsListOfTeamIdsAsync()
        {
            // Arrange
            var seasons = Enumerable.Range(1, 10)
                    .Select(x => new Season(x, $"{x}", default, default))
                    .ToList();
            foreach (var s in seasons)
            {
                foreach (var x in Enumerable.Range(1, 10))
                {
                    s.AddTeam((s.Id * 100) + x, $"#{x}");
                }
            }

            await SeedDbAsync(seasons);
            var season = seasons[5];

            // Act
            using var response = await Client.GetAsync($"{BaseURI}/{season.Id}/teams");

            // Assert
            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadFromJsonAsync<int[]>(SerializerOptions);
            season.Teams.Select(t => t.Id).Should().BeEquivalentTo(actual);
        }
    }
}
