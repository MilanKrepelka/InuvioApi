using ASOL.Inuvio.Api.Client.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASOL.Inuvio.Api.Client
{
    /// <summary>
    /// Implementation of <see cref="IInuvioApiClient"/> that provides access to Inuvio API services.
    /// </summary>
    internal class InuvioApiClient : IInuvioApiClient
    {
        private readonly IInuvioSystemApi _systemApi;
        private readonly IAVAModelsApi _avaModelsApi;

        /// <summary>
        /// Initializes a new instance of the <see cref="InuvioApiClient"/> class.
        /// </summary>
        /// <param name="systemApi">The system API client. Cannot be null.</param>
        /// <param name="avaModelsApi">The AVA Models API client. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when systemApi or avaModelsApi is null.</exception>
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

        /// <inheritdoc/>
        public async Task<ICallResult> CheckConnection(CancellationToken cancellationToken)
        {
            try
            {
                await _systemApi.GetStatusAsync(cancellationToken);
                return CallResult.Success();
            }
            catch (Exception ex)
            {
                return CallResult.Failure(ex.Message);
            }
        }
    }
}
