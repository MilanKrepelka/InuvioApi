using ASOL.Inuvio.Api.Client.Contracts.SystemApi.Responses;
using Refit;

namespace ASOL.Inuvio.Api.Client.SystemApi
{
    /// <summary>
    /// Internal Refit interface for System API - not exposed to consumers
    /// </summary>
    internal interface IInuvioSystemApiRefit
    {
        /// <summary>
        /// Asynchronously retrieves the status of the server, including version information, license details, and the availability of various APIs.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a response with the server status.</returns>
        [Get("/api/v1/core/status")]
        Task<StatusResponse> GetStatusAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves a list of available databases from the server.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a response with the list of
        /// databases.</returns>
        [Get("/api/v1/databases/list")]
        Task<DatabasesResponse> GetDatabasesAsync(CancellationToken cancellationToken = default);
    }

    
}