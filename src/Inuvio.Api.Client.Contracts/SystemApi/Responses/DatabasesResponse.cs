using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ASOL.Inuvio.Api.Client.Contracts.SystemApi.Responses
{
    /// <summary>
    /// Response model for the GetDatabases API endpoint, containing a count of databases and a list of database information records. This model is used to deserialize the JSON response from the API, providing structured access to the database details such as their system names, captions, types, access permissions, and other relevant properties.
    /// </summary>
    public record DatabasesResponse
    {
        /// <summary>
        /// Count represents the total number of databases available in the system. This property provides a quick overview of how many databases are present without needing to iterate through the list of database information records. It is useful for pagination, display purposes, and for clients to understand the scale of the database environment they are interacting with.
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; init; }
        
        /// <summary>
        /// Databases is a list of database information records, each representing a database available in the system. This property provides detailed information about each database, including its system name, caption, type, access permissions, and other relevant properties. It allows clients to access and interact with the individual databases in a structured manner.
        /// </summary>
        [JsonPropertyName("databases")]
        public IReadOnlyList<DatabaseInfo> Databases { get; init; } = Array.Empty<DatabaseInfo>();
    }
}
