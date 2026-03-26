using ASOL.Inuvio.Api.Client.Contracts.SystemApi.Responses;


namespace ASOL.Inuvio.Api.Client.Contracts
{

    /// <summary>
    /// Inuvio SystemApi defines the contract for interacting with the system-related endpoints of the Inuvio API. This interface provides methods for retrieving system status and other system-level information, allowing clients to monitor and manage the health and performance of the Inuvio service.
    /// </summary>
    public interface IInuvioSystemApi
    {
        
        Task<StatusResponse> GetStatus(CancellationToken cancellationToken = default);
        
        
    }
}
