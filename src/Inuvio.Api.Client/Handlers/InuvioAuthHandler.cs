using ASOL.Inuvio.Api.Client.Contracts;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace ASOL.Inuvio.Api.Client.Handlers
{
    /// <summary>
    /// DelegatingHandler that automatically adds Bearer token to all requests
    /// </summary>
    internal class InuvioAuthHandler : DelegatingHandler
    {
        private readonly IInuvioApiTokenProvider _tokenProvider;
        private readonly ILogger<InuvioAuthHandler> _logger;

        public InuvioAuthHandler(
            IInuvioApiTokenProvider tokenProvider,
            ILogger<InuvioAuthHandler> logger)
        {
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                var token = await _tokenProvider.GetTokenAsync();
                
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Token is empty, request may fail");
                    throw new UnauthorizedAccessException("Failed to obtain authentication token");
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                _logger.LogDebug("Added Bearer token to request: {Method} {Uri}", request.Method, request.RequestUri);

                var response = await base.SendAsync(request, cancellationToken);

                // Handle 401 Unauthorized
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogWarning("Received 401 Unauthorized, invalidating token");
                    await _tokenProvider.InvalidateAccessTokenAsync();
                    
                    // Optionally revoke the token
                    try
                    {
                        await _tokenProvider.RevokeTokenAsync(token);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to revoke token, but continuing");
                    }
                }

                return response;
            }
            catch (Exception ex) when (ex is not UnauthorizedAccessException)
            {
                _logger.LogError(ex, "Error in InuvioAuthHandler: {ErrorMessage}", ex.Message);
                throw;
            }
        }
    }
}