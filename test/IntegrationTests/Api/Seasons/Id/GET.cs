namespace IntegrationTests.Api.Seasons.Id
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
    /// Integration tests for <c>GET</c> requests to <c>/api/seasons/{id}</c>.
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
            using var response = await Client.GetAsync($"{BaseURI}/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// Ensures the response contains the requested season.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsRequestedSeason()
        {
            // Arrange
            var seasons = Enumerable.Range(1, 10)
                .Select(x => new Season(x, $"{x}", default, default))
                .ToList();
            await SeedDbAsync(seasons);
            var season = seasons[5];

            // Act
            using var response = await Client.GetAsync($"{BaseURI}/{season.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadFromJsonAsync<SeasonModel>(SerializerOptions);
            Assert.Equal(Mapper.Map<SeasonModel>(season), actual);
        }
    }
}
