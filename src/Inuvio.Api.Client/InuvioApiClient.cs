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

        public InuvioApiClient(
            IInuvioSystemApi systemApi)
            
        {
            _systemApi = systemApi ?? throw new ArgumentNullException(nameof(systemApi));
        }

        public IInuvioSystemApi InuvioSystemApi => _systemApi;

        /// <summary>
        /// Checks whether a connection to the target service or resource can be established.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ICallResult"/> indicating the success or failure of the connection check.</returns>
        /// <exception cref="NotImplementedException">Thrown in all cases. This method is not yet implemented.</exception>
        public async Task<ICallResult> CheckConnection(CancellationToken cancellationToken)
        {
            try
            {
                await _systemApi.GetStatus(cancellationToken);  // ✅ Přes public interface
                return new CallResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new CallResult { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }
    }
}
