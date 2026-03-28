using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ASOL.Inuvio.Api.Client.Contracts.AVAModelsApi.Responses
{
    /// <summary>
    /// Represents information about a single AVA model including its capabilities.
    /// </summary>
    public sealed record AVAModelInfo
    {
        /// <summary>
        /// Gets the unique identifier of the model.
        /// </summary>
        [JsonPropertyName("modelId")]
        public string ModelId { get; init; } = string.Empty;

        /// <summary>
        /// Gets the code identifier of the model.
        /// </summary>
        [JsonPropertyName("modelCode")]
        public string ModelCode { get; init; } = string.Empty;

        /// <summary>
        /// Gets a value indicating whether write operations are implemented for this model.
        /// </summary>
        [JsonPropertyName("isWriteImplemented")]
        public bool IsWriteImplemented { get; init; }

        /// <summary>
        /// Gets a value indicating whether read operations are implemented for this model.
        /// </summary>
        [JsonPropertyName("isReadImplemented")]
        public bool IsReadImplemented { get; init; }

        /// <summary>
        /// Gets a value indicating whether delete operations are implemented for this model.
        /// </summary>
        [JsonPropertyName("isDeleteImplemented")]
        public bool IsDeleteImplemented { get; init; }
    }
}
