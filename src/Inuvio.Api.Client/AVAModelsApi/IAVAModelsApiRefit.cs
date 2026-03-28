using ASOL.Inuvio.Api.Client.Contracts.AVAModelsApi.Responses;
using Refit;

namespace ASOL.Inuvio.Api.Client.AVAModelsApi
{
    /// <summary>
    /// Internal Refit interface for AVA Models API - not exposed to consumers
    /// </summary>
    internal interface IAVAModelsApiRefit
    {
        [Get("/api/v1/avaModels/list")]
        Task<AVAModelsResponse> GetAVAModelsAsync(CancellationToken cancellationToken = default);
    }
}