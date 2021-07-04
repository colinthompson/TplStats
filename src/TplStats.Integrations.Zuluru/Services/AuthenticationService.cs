namespace TplStats.Integrations.Zuluru.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Flurl.Http;
    using Flurl.Http.Configuration;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using TplStats.Integrations.Zuluru.Interfaces;
    using TplStats.Integrations.Zuluru.Options;

    /// <summary>
    /// Service that manages authenticating with the Zuluru server.
    /// </summary>
    public class AuthenticationService : ZuluruServiceBase, IAuthenticationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="zuluruOptions">Zuluru configuration options.</param>
        /// <param name="flurlClientFactory">Flurl client factory.</param>
        /// <param name="cache">Cache.</param>
        public AuthenticationService(IOptions<ZuluruOptions> zuluruOptions, IFlurlClientFactory flurlClientFactory, IMemoryCache cache)
            : base(zuluruOptions, flurlClientFactory)
        {
            Cache = cache;
        }

        private IMemoryCache Cache { get; }

        /// <inheritdoc/>
        public Task<string> GetAuthorizationTokenAsync(CancellationToken cancellationToken)
        {
            return Cache.GetOrCreateAsync("ZuluruAuthToken", async cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(6);

                var data = new Dictionary<string, string>
                {
                    { Options.Authentication.UsernameParam, Options.Authentication.Username },
                    { Options.Authentication.PasswordParam, Options.Authentication.Password },
                };
                using var response = HttpClient
                    .Request(Options.GetAuthenticationUrl())
                    .PostJsonAsync(data, cancellationToken: cancellationToken);

                var authResponse = await response.ReceiveJson<AuthResponse>();

                return authResponse.Data["token"];
            });
        }

        private class AuthResponse
        {
            public bool Success { get; set; }

            public Dictionary<string, string> Data { get; set; } = null!;
        }
    }
}
