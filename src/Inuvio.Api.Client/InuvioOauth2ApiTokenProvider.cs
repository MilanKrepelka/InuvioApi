using ASOL.Inuvio.Api.Client.Options;
using ASOL.Inuvio.Api.Client.Contracts;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace ASOL.Inuvio.Api.Client
{
    /// <summary>
    /// Provides OAuth2-based token authentication for iNuvio API.
    /// </summary>
    internal class InuvioOauth2ApiTokenProvider : IInuvioApiTokenProvider, IDisposable
    {
        private const string HttpClientName = "iNuvioAPIAuth";

        private class AuthTokenResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; } = string.Empty;

            [JsonPropertyName("token_type")]
            public string TokenType { get; set; } = string.Empty;

            [JsonPropertyName("expires_in")]
            public string ExpiresIn { get; set; } = string.Empty;

            [JsonPropertyName("refresh_token")]
            public string RefreshToken { get; set; } = string.Empty;
        }

        private readonly InuvioApiConnectionOptions _connectionOptions;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IInuvioApiTokenGenerator _tokenGenerator;
        private readonly SemaphoreSlim _tokenLock = new(1, 1);

        /// <summary>
        /// Refresh token for obtaining new access tokens.
        /// </summary>
        private JwtSecurityToken _refreshToken = null!;

        /// <summary>
        /// Currently valid access token. Refreshed automatically when expired.
        /// </summary>
        private JwtSecurityToken? _accessToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="InuvioOauth2ApiTokenProvider"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory for creating HTTP clients.</param>
        /// <param name="connectionOptions">The connection options containing credentials and server URL.</param>
        /// <param name="tokenGenerator">The token generator for creating client authentication tokens.</param>
        /// <exception cref="ArgumentNullException">Thrown when any required parameter is null.</exception>
        public InuvioOauth2ApiTokenProvider(
            IHttpClientFactory httpClientFactory,
            IOptions<InuvioApiConnectionOptions> connectionOptions,
            IInuvioApiTokenGenerator tokenGenerator)
        {
            if (connectionOptions?.Value is null)
            {
                throw new ArgumentNullException(nameof(connectionOptions));
            }

            _connectionOptions = connectionOptions.Value;
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
        }

        private string Oauth2Url => _connectionOptions.ServerUrlWithoutTrailingSlash + "/oauth/token";
        private string Oauth2RevokeUrl => _connectionOptions.ServerUrlWithoutTrailingSlash + "/oauth/revoke";

        /// <summary>
        /// Obtains new access and refresh tokens from the OAuth2 endpoint.
        /// </summary>
        /// <returns>A tuple containing the access token and refresh token.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when authentication fails.</exception>
        /// <exception cref="InvalidOperationException">Thrown when token deserialization fails.</exception>
        private async Task<(JwtSecurityToken AccessToken, JwtSecurityToken RefreshToken)> GetNewTokensAsync()
        {
            var formData = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "password"),
                new("username", _connectionOptions.UserName),
                new("password", _connectionOptions.Password),
                new("clientId", _tokenGenerator.GenerateToken())
            };

            using var client = _httpClientFactory.CreateClient(HttpClientName);
            using var req = new HttpRequestMessage(HttpMethod.Post, Oauth2Url) 
            { 
                Content = new FormUrlEncodedContent(formData) 
            };
            
            var response = await client.SendAsync(req);
            var jsonString = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                throw new UnauthorizedAccessException(
                    $"User '{_connectionOptions.UserName}' is unauthorized to {Oauth2Url}. " +
                    $"Status: {response.StatusCode}, Reason: {jsonString}");
            }
            
            var authResponse = System.Text.Json.JsonSerializer.Deserialize<AuthTokenResponse>(jsonString);
            if (authResponse == null || string.IsNullOrEmpty(authResponse.AccessToken))
            {
                throw new InvalidOperationException("Failed to deserialize token response or access token is empty.");
            }
            
            return (new JwtSecurityToken(authResponse.AccessToken),
                    string.IsNullOrEmpty(authResponse.RefreshToken) 
                        ? new JwtSecurityToken(authResponse.AccessToken) 
                        : new JwtSecurityToken(authResponse.RefreshToken));
        }

        /// <summary>
        /// Retrieves a valid access token, obtaining a new one if necessary.
        /// </summary>
        /// <returns>A valid access token string.</returns>
        /// <remarks>This method ensures thread-safe token management and reuses tokens until expiration.</remarks>
        public async Task<string> GetTokenAsync()
        {
            await _tokenLock.WaitAsync();
            try
            {
                // Return existing token if still valid
                if (_accessToken != null && 
                    _accessToken.ValidTo.AddMinutes(-1).ToUniversalTime() > DateTime.UtcNow)
                {
                    return _accessToken.RawData;
                }
                
                // Otherwise obtain new tokens
                var tokens = await GetNewTokensAsync();
                _accessToken = tokens.AccessToken;
                _refreshToken = tokens.RefreshToken;
                return _accessToken.RawData;
            }
            finally
            {
                _tokenLock.Release();
            }
        }

        /// <summary>
        /// Revokes the specified token at the OAuth2 endpoint.
        /// </summary>
        /// <param name="token">The token to revoke.</param>
        /// <returns>The HTTP status code of the revocation operation.</returns>
        public async Task<HttpStatusCode> RevokeTokenAsync(string token)
        {
            var formData = new List<KeyValuePair<string, string>>
            {
                new("token", token),
                new("token_type_hint", string.Empty)
            };

            using var client = _httpClientFactory.CreateClient(HttpClientName);
            using var req = new HttpRequestMessage(HttpMethod.Post, Oauth2RevokeUrl) 
            { 
                Content = new FormUrlEncodedContent(formData) 
            };
            
            var response = await client.SendAsync(req);
            return response.StatusCode;
        }

        /// <summary>
        /// Invalidates the current access token, forcing a new token to be obtained on the next request.
        /// </summary>
        /// <returns>A completed task.</returns>
        /// <remarks>Use this when connection is lost or the token needs to be refreshed immediately.</remarks>
        public Task InvalidateAccessTokenAsync()
        {
            _accessToken = null;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Releases resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            _tokenLock?.Dispose();
        }
    }
}
