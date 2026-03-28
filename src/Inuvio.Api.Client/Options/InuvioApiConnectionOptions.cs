namespace ASOL.Inuvio.Api.Client.Options
{
    /// <summary>
    /// Configuration options for connecting to the iNuvio API.
    /// </summary>
    public class InuvioApiConnectionOptions
    {
        /// <summary>
        /// Username for iNuvio API connection.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Password for iNuvio API connection.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// URL to iNuvio API endpoint.
        /// </summary>
        public string ServerUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets the <see cref="ServerUrl"/> without trailing slash.
        /// </summary>
        public string ServerUrlWithoutTrailingSlash => ServerUrl.TrimEnd('/');

        /// <summary>
        /// Returns a string representation of the connection options with masked password.
        /// </summary>
        /// <returns>A string containing server URL, username, and masked password.</returns>
        public override string ToString()
        {
            return $"ServerUrl:{ServerUrl}, UserName:{UserName}, Password:**??**";
        }

        /// <summary>
        /// Parses the server URL to extract the base URL and port number.
        /// </summary>
        /// <param name="serverUrl">The server URL to parse.</param>
        /// <returns>A tuple containing the server URL without port and the port number.</returns>
        public static (string ServerUrlWoPort, string Port) ParseServerUrl(string serverUrl)
        {
            if (string.IsNullOrEmpty(serverUrl))
            {
                return (string.Empty, string.Empty);
            }

            var idx = serverUrl.LastIndexOf(':');
            if (idx == -1)
            {
                return (serverUrl, string.Empty);
            }
            else
            {
                var port = serverUrl.Substring(idx + 1);
                if (int.TryParse(port, out _))
                {
                    return (serverUrl.Substring(0, idx), port);
                }
                else
                {
                    return (serverUrl, string.Empty);
                }
            }
        }

        /// <summary>
        /// Parses the login name to extract the domain and username.
        /// </summary>
        /// <param name="loginName">The login name to parse (format: DOMAIN\Username).</param>
        /// <returns>A tuple containing the domain name and username.</returns>
        public static (string DomainName, string UserName) ParseLoginName(string loginName)
        {
            if (string.IsNullOrEmpty(loginName))
            {
                return (string.Empty, string.Empty);
            }

            var idx = loginName.IndexOf('\\');
            return idx == -1
                ? (string.Empty, loginName)
                : (loginName.Substring(0, idx), loginName.Substring(idx + 1));
        }
    }
}