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

        /// <summary>
        /// Asynchronously retrieves the list of system databases from the Inuvio API. Logs the operation and handles any API exceptions that may occur.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a response with the list of system databases.</returns>
        public async Task<DatabasesResponse> GetDatabasesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Getting system databases");
                var result = await _refitClient.GetDatabasesAsync(cancellationToken);
                _logger.LogDebug("Successfully retrieved system databases");
                return result;
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, "Failed to get system databases. Status: {StatusCode}", ex.StatusCode);
                throw;
            }
        }

        /// <summary>
        /// Asynchronously retrieves the current system status.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="StatusResponse"/>
        /// object with the current system status.</returns>
        public async Task<StatusResponse> GetStatusAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Getting system status");
                var result = await _refitClient.GetStatusAsync(cancellationToken);
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
