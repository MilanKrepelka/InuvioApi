using System.Net;
using System.Threading.Tasks;

namespace ASOL.Inuvio.Api.Client.Contracts
{
    /// <summary>
    /// Provides token authentication for iNuvio API access.
    /// </summary>
    public interface IInuvioApiTokenProvider
    {
        /// <summary>
        /// Retrieves a valid access token for API authentication.
        /// </summary>
        /// <returns>A valid access token string.</returns>
        Task<string> GetTokenAsync();

        /// <summary>
        /// Revokes the specified token at the iNuvio API endpoint.
        /// </summary>
        /// <param name="token">The token to revoke.</param>
        /// <returns>The HTTP status code of the revocation operation.</returns>
        Task<HttpStatusCode> RevokeTokenAsync(string token);

        /// <summary>
        /// Invalidates the current access token, forcing a new token to be obtained.
        /// </summary>
        /// <returns>A completed task.</returns>
        /// <remarks>
        /// Use this when the token needs to be invalidated, such as after connection loss or security concerns.
        /// </remarks>
        Task InvalidateAccessTokenAsync();
    }
}
