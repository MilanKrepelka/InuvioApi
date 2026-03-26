using ASOL.Inuvio.Api.Client.Contracts.SystemApi.Responses;
using ASOL.Inuvio.Api.Client.Contracts;
using Microsoft.Extensions.Logging;

namespace ASOL.Inuvio.Api.Client.SystemApi
{
    /// <summary>
    /// Implementation of IInuvioSystemApiRefit that delegates to Refit client
    /// </summary>
    internal class InuvioSystemApi : IInuvioSystemApi
    {
        private readonly IInuvioSystemApiRefit _refitClient;
        private readonly ILogger<InuvioSystemApi> _logger;

        /// <summary>
        /// Initializes a new instance of the InuvioSystemApi class with the specified Refit client and logger.
        /// </summary>
        /// <param name="refitClient">The Refit client used to communicate with the Inuvio system API. Cannot be null.</param>
        /// <param name="logger">The logger instance used for logging diagnostic messages. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if refitClient or logger is null.</exception>
        public InuvioSystemApi(
            IInuvioSystemApiRefit refitClient,
            ILogger<InuvioSystemApi> logger)
        {
            _refitClient = refitClient ?? throw new ArgumentNullException(nameof(refitClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>   
        public async Task<StatusResponse> GetStatus(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Getting system status");
                var result = await _refitClient.GetStatus(cancellationToken);
                _logger.LogDebug("Successfully retrieved system status");
                return result;
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, "Failed to get system status. Status: {StatusCode}", ex.StatusCode);
                throw;
            }
        }
    }
}
