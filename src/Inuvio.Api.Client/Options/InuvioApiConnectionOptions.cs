namespace ASOL.Inuvio.Api.Client.Options
{
    public class InuvioApiConnectionOptions
    {
        /// <summary>
        /// Uživatel pro připojení k iNuvio API
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Heslo pro připojení k iNuvio API
        /// </summary>
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// Url k iNuvio API
        /// </summary>
        public string ServerUrl { get; set; } = string.Empty;

        /// <summary>
        /// <see cref="ServerUrl"/> bez koncového lomítka
        /// </summary>
        public string ServerUrlWithoutTrailingSlash => ServerUrl.TrimEnd('/');


        public InuvioApiConnectionOptions()
        {

        }

        override public string ToString()
        {
            return $"ServerUrl:{ServerUrl}, UserName:{UserName}, Password:**??**";
        }

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