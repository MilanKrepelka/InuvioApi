using ASOL.Inuvio.Api.Client.Contracts.SystemApi.Responses;
using Refit;

namespace ASOL.Inuvio.Api.Client.SystemApi
{
    /// <summary>
    /// Internal Refit interface for System API - not exposed to consumers
    /// </summary>
    internal interface IInuvioSystemApiRefit
    {
        [Get("/api/v1/core/status")]
        Task<StatusResponse> GetStatusAsync(CancellationToken cancellationToken = default);
    }
}