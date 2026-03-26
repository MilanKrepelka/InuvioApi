using System;
using System.Collections.Generic;
using System.Text;

namespace ASOL.Inuvio.Api.Client.Contracts
{
    /// <summary>
    /// Rozhraní které reprezentuje funkcionalitu generování JWT tokenu pro autentikaci k iNuvio eServeru
    /// </summary>
    public interface IInuvioApiTokenGenerator
    {
        /// <summary>
        /// Vygeneruje token
        /// </summary>
        /// <returns>Token</returns>
        string GenerateToken();
    }
}
