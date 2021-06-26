namespace IntegrationTests.Api.Seasons
{
    using System.Collections.Generic;
    using System.Linq;
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
    /// Integration test for <c>GET</c> requests to <c>/api/seasons</c>.
    /// </summary>
    public class GET : IntegrationTestBase
    {
        private const string URI = "/api/seasons";

        /// <summary>
        /// Initializes a new instance of the <see cref="GET"/> class.
        /// </summary>
        /// <param name="testOutputHelper">Test output helper.</param>
        public GET(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Ensures an empty list is returned when no <see cref="Season"/> entities exist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task EmptyListWhenNoSeasonsAsync()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            using var response = await client.GetAsync(URI);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Empty(await response.Content.ReadFromJsonAsync<ICollection<SeasonModel>>());
        }

        /// <summary>
        /// Ensures the response contains all <see cref="Season"/> entities.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsAllKnownSeasonsAsync()
        {
            // Arrange
            var seasons = Enumerable.Range(1, 10)
                .Select(x => new Season(x, $"Season #{x}", new LocalDate(2021, 1, x), new LocalDate(2021, 1, x + 10)))
                .ToList();
            await SeedDbAsync(seasons);

            // Act
            using var response = await Client.GetAsync(URI);

            // Assert
            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadFromJsonAsync<SeasonModel[]>(SerializerOptions);
            Assert.Equal(seasons.AsQueryable().ProjectTo<SeasonModel>(Mapper.ConfigurationProvider), actual);
        }
    }
}
