namespace UnitTests.Integrations.Zuluru.Services.AuthenticationServiceTests
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using Flurl.Http.Testing;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.DependencyInjection;
    using TplStats.Integrations.Zuluru.Interfaces;
    using TplStats.Integrations.Zuluru.Services;
    using UnitTests.Integrations.Zuluru.Services.Helpers;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="AuthenticationService.GetAuthorizationTokenAsync(CancellationToken)"/>.
    /// </summary>
    public class GetAuthorizationTokenAsync : ZuluruServiceTestBase<IAuthenticationService, AuthenticationService>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAuthorizationTokenAsync"/> class.
        /// </summary>
        public GetAuthorizationTokenAsync() => Service = ServiceProvider.GetRequiredService<IAuthenticationService>();

        private IAuthenticationService Service { get; }

        /// <summary>
        /// Ensures the authorization service makes a POST request to the correct endpoint.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CorrectEndpointIsCalledAsync()
        {
            // Arrange
            var data = new
            {
                success = "true",
                data = new
                {
                    token = "authTokenValue",
                },
            };
            using var httpTest = new HttpTest();
            httpTest.RespondWithJson(data);

            // Act
            await Service.GetAuthorizationTokenAsync(default);

            // Assert
            httpTest
                .ShouldHaveCalled(Configuration.GetAuthenticationUrl())
                .WithVerb(HttpMethod.Post)
                .WithContentType(MediaTypeNames.Application.Json)
                .WithRequestJson(GetRequestBody());
        }

        /// <summary>
        /// Ensures the authentication token is cached.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TokenIsCached()
        {
            // Arrange
            var data = new
            {
                success = "true",
                data = new
                {
                    token = "authTokenValue",
                },
            };
            using var httpTest = new HttpTest();
            httpTest.RespondWithJson(data);
            var cache = ServiceProvider.GetRequiredService<IMemoryCache>();

            // Act
            await Service.GetAuthorizationTokenAsync(default);

            // Assert
            Assert.NotNull(cache.Get<string>("ZuluruAuthToken"));
        }

        /// <summary>
        /// Ensures the returned authentication token is correctly formatted.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsAuthTokenAsync()
        {
            // Arrange
            var data = new
            {
                success = "true",
                data = new
                {
                    token = "authTokenValue",
                },
            };
            using var httpTest = new HttpTest();
            httpTest.RespondWithJson(data);
            var cache = ServiceProvider.GetRequiredService<IMemoryCache>();

            // Act
            var token = await Service.GetAuthorizationTokenAsync(default);

            // Assert
            Assert.Equal(data.data.token, token);
        }

        private Dictionary<string, string> GetRequestBody()
        {
            var auth = Configuration.Authentication;
            return new Dictionary<string, string>
            {
                { auth.UsernameParam, auth.Username },
                { auth.PasswordParam, auth.Password },
            };
        }
    }
}
