namespace TplStats.Integrations.Zuluru.Services
{
    using System;
    using Flurl.Http;
    using Flurl.Http.Configuration;
    using Microsoft.Extensions.Options;
    using TplStats.Integrations.Zuluru.Options;

    /// <summary>
    /// Base class for services that integrate TPL Stats with a Zuluru server.
    /// </summary>
    public abstract class ZuluruServiceBase : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZuluruServiceBase"/> class.
        /// </summary>
        /// <param name="zuluruOptions">Zuluru configuration options.</param>
        /// <param name="flurlClientFactory">Flurl client factory.</param>
        protected ZuluruServiceBase(IOptions<ZuluruOptions> zuluruOptions, IFlurlClientFactory flurlClientFactory)
        {
            Options = zuluruOptions.Value;
            HttpClient = flurlClientFactory.Get(Options.BaseUrl);
        }

        /// <summary>
        /// Gets the Zuluru configuration options.
        /// </summary>
        protected ZuluruOptions Options { get; }

        /// <summary>
        /// Gets the HTTP Client.
        /// </summary>
        protected IFlurlClient HttpClient { get; }

        /// <inheritdoc/>
        public virtual void Dispose() => HttpClient.Dispose();
    }
}
