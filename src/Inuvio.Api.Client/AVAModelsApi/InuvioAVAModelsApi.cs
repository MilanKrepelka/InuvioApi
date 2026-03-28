using ASOL.Inuvio.Api.Client.Contracts;
using ASOL.Inuvio.Api.Client.Contracts.AVAModelsApi.Responses;
using Microsoft.Extensions.Logging;

namespace ASOL.Inuvio.Api.Client.AVAModelsApi
{
    /// <summary>
    /// Implementation of <see cref="IAVAModelsApi"/> that delegates to Refit client
    /// </summary>
    internal class InuvioAVAModelsApi : IAVAModelsApi
    {
        private readonly IAVAModelsApiRefit _refitClient;
        private readonly ILogger<InuvioAVAModelsApi> _logger;

        /// <summary>
        /// Initializes a new instance of the AVAModelsApi class with the specified Refit client and logger.
        /// </summary>
        /// <param name="refitClient">The Refit client used to communicate with the Inuvio AVA Models API. Cannot be null.</param>
        /// <param name="logger">The logger instance used for logging diagnostic messages. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if refitClient or logger is null.</exception>
        public InuvioAVAModelsApi(
            IAVAModelsApiRefit refitClient,
            ILogger<InuvioAVAModelsApi> logger)
        {
            _refitClient = refitClient ?? throw new ArgumentNullException(nameof(refitClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>   
        public async Task<AVAModelsResponse> GetAVAModelsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Getting AVA models definitions");
                var result = await _refitClient.GetAVAModelsAsync(cancellationToken);
                _logger.LogDebug("Successfully retrieved AVA models definitions");
                return result;
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, "Failed to get AVA models definitions. Status: {StatusCode}", ex.StatusCode);
                throw;
            }
        }
    }
}
