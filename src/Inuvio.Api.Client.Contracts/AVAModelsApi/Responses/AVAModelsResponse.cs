using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ASOL.Inuvio.Api.Client.Contracts.AVAModelsApi.Responses
{
    /// <summary>
    /// Represents the response containing a collection of AVA models.
    /// </summary>
    public sealed record AVAModelsResponse
    {
        /// <summary>
        /// Gets the collection of AVA models.
        /// </summary>
        [JsonPropertyName("models")]
        public IReadOnlyList<AVAModelInfo> Models { get; init; } = [];
    }
}
