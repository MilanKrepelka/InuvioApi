using System;
using System.Collections.Generic;
using System.Text;

namespace ASOL.Inuvio.Api.Client.Contracts
{
    /// <summary>
    /// Rozhraní pro Inuvio API klienta, který bude zajišťovat komunikaci s Inuvio API.
    /// </summary>
    public interface IInuvioApiClient
    {

        /// <summary>
        /// Gets the system API interface
        /// </summary>
        IInuvioSystemApi InuvioSystemApi { get; }

        /// <summary>
        /// Asynchronously checks the status of the current connection.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an object describing the outcome
        /// of the connection check.</returns>
        Task<ICallResult> CheckConnection(CancellationToken cancellationToken);
    }
}
