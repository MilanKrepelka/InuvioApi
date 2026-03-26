using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ASOL.Inuvio.Api.Client.Contracts.SystemApi.Responses
{
    /// <summary>
    /// Represents the result of a status Inuvio API call, indicating the current status of the service or system. This response may include information about the health, uptime, or any issues affecting the service. The specific details included in this response can vary based on the implementation of the API and the information it provides about the system's status.
    /// </summary>
    public record StatusResponse
    {
        /// <summary>
        /// Version of the Inuvio system
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; init; } = string.Empty;

        /// <summary>
        /// Licence identifier
        /// </summary>
        [JsonPropertyName("licence")]
        public string Licence { get; init; } = string.Empty;

        /// <summary>
        /// SQL Server version and details
        /// </summary>
        [JsonPropertyName("sqlServer")]
        public string SqlServer { get; init; } = string.Empty;

        /// <summary>
        /// System ID code
        /// </summary>
        [JsonPropertyName("idCode")]
        public string IdCode { get; init; } = string.Empty;

        /// <summary>
        /// Database server name
        /// </summary>
        [JsonPropertyName("server")]
        public string Server { get; init; } = string.Empty;

        /// <summary>
        /// Indicates whether System API is enabled
        /// </summary>
        [JsonPropertyName("systemAPI")]
        public bool SystemAPI { get; init; }

        /// <summary>
        /// Indicates whether eShop API is enabled
        /// </summary>
        [JsonPropertyName("eShopAPI")]
        public bool EShopAPI { get; init; }

        /// <summary>
        /// Indicates whether Received Invoices API is enabled
        /// </summary>
        [JsonPropertyName("receivedInvoicesAPI")]
        public bool ReceivedInvoicesAPI { get; init; }

        /// <summary>
        /// Indicates whether Issued Invoices API is enabled
        /// </summary>
        [JsonPropertyName("issuedInvoicesAPI")]
        public bool IssuedInvoicesAPI { get; init; }

        /// <summary>
        /// Indicates whether Job Orders API is enabled
        /// </summary>
        [JsonPropertyName("jobOrdersAPI")]
        public bool JobOrdersAPI { get; init; }

        /// <summary>
        /// Indicates whether Human Resources API is enabled
        /// </summary>
        [JsonPropertyName("humanResourcesAPI")]
        public bool HumanResourcesAPI { get; init; }
    }
}
