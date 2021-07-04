namespace TplStats.Integrations.Zuluru.Options
{
    /// <summary>
    /// Configuration options for authenticating with a Zuluru server.
    /// </summary>
    public class AuthenticationOptions
    {
        /// <summary>
        /// Gets or sets the authentication endpoint.
        /// </summary>
        public string Endpoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the username parameter.
        /// </summary>
        public string UsernameParam { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the password parameter.
        /// </summary>
        public string PasswordParam { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the username.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the password.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
