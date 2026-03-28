using ASOL.Inuvio.Api.Client.Contracts;
using ASOL.Inuvio.Api.Client.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASOL.Inuvio.Api.Client
{
    // InuvioApiClient.cs - INJECT adapter, ne Refit interface!
    internal class InuvioApiClient : IInuvioApiClient
    {
        private readonly IInuvioSystemApi _systemApi;
        private readonly IAVAModelsApi _avaModelsApi;

        public InuvioApiClient(
            IInuvioSystemApi systemApi,
            IAVAModelsApi avaModelsApi)
            
        {
            _systemApi = systemApi ?? throw new ArgumentNullException(nameof(systemApi));
            _avaModelsApi = avaModelsApi ?? throw new ArgumentNullException(nameof(avaModelsApi));
        }

        /// <summary>
        /// Gets the Inuvio System API client.
        /// </summary>
        public IInuvioSystemApi InuvioSystemApi => _systemApi;
        /// <summary>
        /// Gets the AVA Models API client.
        /// </summary>  
        public IAVAModelsApi AVAModelsApi => _avaModelsApi;

        /// <summary>
        /// Checks whether a connection to the target service or resource can be established.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ICallResult"/> indicating the success or failure of the connection check.</returns>
        /// <exception cref="NotImplementedException">Thrown in all cases. This method is not yet implemented.</exception>
        public async Task<ICallResult> CheckConnection(CancellationToken cancellationToken)
        {
            try
            {
                await _systemApi.GetStatusAsync(cancellationToken);  // ✅ Přes public interface
                return new CallResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new CallResult { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }
    }
}
