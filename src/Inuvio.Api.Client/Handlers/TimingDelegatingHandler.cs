using Microsoft.Extensions.Logging;
namespace ASOL.Inuvio.Api.Client.Handlers
{
    /// <summary>
    /// DelegatingHandler that measures the time taken for an HTTP request and logs the duration along with the request method, URI, and response status code.
    /// </summary>
    internal class TimingDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<TimingDelegatingHandler> _logger;

        public TimingDelegatingHandler(ILogger<TimingDelegatingHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var requestUri = request.RequestUri?.ToString() ?? "unknown";

            try
            {
                _logger.LogDebug("Starting HTTP request: {Method} {Uri}", request.Method, requestUri);

                var response = await base.SendAsync(request, cancellationToken);

                stopwatch.Stop();
                _logger.LogInformation(
                    "HTTP {Method} {Uri} completed with {StatusCode} in {ElapsedMs}ms",
                    request.Method,
                    requestUri,
                    (int)response.StatusCode,
                    stopwatch.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex,
                    "HTTP {Method} {Uri} failed after {ElapsedMs}ms",
                    request.Method,
                    requestUri,
                    stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}