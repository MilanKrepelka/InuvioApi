using ASOL.Inuvio.Api.Client.Contracts.AVAModelsApi.Responses;


namespace ASOL.Inuvio.Api.Client.Contracts
{

    /// <summary>
    /// Inuvio AVA Models API client interface. Provides methods to interact with the iNuvio API related to AVA models.
    /// </summary>
    public interface IAVAModelsApi
    {
        /// <summary>
        /// Asynchronously retrieves the AVA models definitions.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a AVAModelsResponse object with the
        /// status information of AVA models.</returns>
        Task<AVAModelsResponse> GetAVAModelsAsync(CancellationToken cancellationToken = default);
    }
}
