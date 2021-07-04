namespace TplStats.Integrations.Zuluru.Options
{
    /// <summary>
    /// Configuration options for TPL Stat's Zuluru integration.
    /// </summary>
    public class ZuluruOptions
    {
        /// <summary>
        /// Gets or sets the root URL of the Zuluru server.
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the authentication options.
        /// </summary>
        public AuthenticationOptions Authentication { get; set; } = null!;

        /// <summary>
        /// Retrieves the absolute URl to the authentication endpoint.
        /// </summary>
        /// <returns>The absolute url to the authentication endpoint.</returns>
        public string GetAuthenticationUrl() => $"{BaseUrl}/{Authentication.Endpoint}";
    }
}
