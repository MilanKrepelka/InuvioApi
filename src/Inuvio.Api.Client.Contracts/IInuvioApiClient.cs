using ASOL.Inuvio.Api.Client.Contracts.SystemApi.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASOL.Inuvio.Api.Client.Contracts
{
    /// <summary>
    /// Inuvio API client interface defines the contract for interacting with the Inuvio API. It provides access to various API interfaces, such as the system API and AVA models API, allowing clients to perform operations related to system status, AVA model management, and other functionalities offered by the Inuvio service. Additionally, it includes a method for checking the connection status to ensure that the client can successfully communicate with the Inuvio API.
    /// </summary>
    public interface IInuvioApiClient
    {
        /// <summary>
        /// Gets the system API interface
        /// </summary>
        IInuvioSystemApi InuvioSystemApi { get; }

        /// <summary>
        /// Gets the AVA Models API interface
        /// </summary>
        IAVAModelsApi AVAModelsApi { get; }

        /// <summary>
        /// Asynchronously checks the status of the current connection.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ICallResult"/> indicating the success or failure of the connection check.</returns>
        Task<ICallResult> CheckConnection(CancellationToken cancellationToken);
    }
}
