namespace TplStats.Integrations.Zuluru.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Service that manages authenticating with the Zuluru server.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Retrieves a bearer token that can be used to authenticate with the Zuluru API.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The bearer token.</returns>
        Task<string> GetAuthorizationTokenAsync(CancellationToken cancellationToken);
    }
}
