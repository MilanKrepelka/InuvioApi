using System;
using System.Collections.Generic;
using System.Text;

namespace ASOL.Inuvio.Api.Client.Contracts
{
    /// <summary>
    /// Interface for generating API tokens for iNuvio API authentication.
    /// </summary>
    public interface IInuvioApiTokenGenerator
    {
        /// <summary>
        /// Generates a new API token or retrieves an existing one for authenticating with the iNuvio API.
        /// </summary>
        /// <returns>The generated API token.</returns>
        string GenerateToken();
    }
}
