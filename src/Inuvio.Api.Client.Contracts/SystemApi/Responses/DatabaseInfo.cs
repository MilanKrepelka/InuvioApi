using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ASOL.Inuvio.Api.Client.Contracts.SystemApi.Responses
{
    /// <summary>
    /// DatabaseInfo represents the details of a database in the iNuvio system. It includes properties such as the database number, system name, caption, type, access permissions, auto-close setting, online status, notification settings, color, and icon ID. This record is used to deserialize the JSON response from the API when retrieving information about databases in the system. Each property corresponds to a specific aspect of the database's configuration and status within the iNuvio environment.
    /// </summary>
    public record DatabaseInfo
    {
        /// <summary>
        /// Gets the database number associated with this instance.
        /// </summary>
        [JsonPropertyName("dbNumber")]
        public int DbNumber { get; init; }

        /// <summary>
        /// Gets the system name of the database.
        /// </summary>  
        [JsonPropertyName("dbSysName")]
        public string DbSysName { get; init; } = string.Empty;

        /// <summary>
        /// Gets the caption of the database.
        /// </summary>
        [JsonPropertyName("dbCaption")]
        public string DbCaption { get; init; } = string.Empty;

        /// <summary>
        /// Gets the type of the database.  
        /// </summary>
        ///<remarks>Better to use an enum for dbType to improve readability and maintainability.</remarks>
        [JsonPropertyName("dbType")]
        public int DbType { get; init; }

        /// <summary>
        /// Gets a value indicating whether the database user has access to the database.
        /// </summary>
        [JsonPropertyName("dbHasDBAccess")]
        public bool DbHasDBAccess { get; init; }

        /// <summary>
        /// Gets a value indicating whether the database connection is automatically closed when it is no longer needed.
        /// </summary>
        [JsonPropertyName("dbAutoClose")]
        public bool DbAutoClose { get; init; }

        /// <summary>
        /// Gets a value indicating whether the database is currently online.
        /// </summary>
        [JsonPropertyName("dbIsOnline")]
        public bool DbIsOnline { get; init; }

        /// <summary>
        /// Gets a value indicating whether database notifications are enabled.
        /// </summary>
        [JsonPropertyName("dbNotifications")]
        public bool DbNotifications { get; init; }

        /// <summary>
        /// Gets the color value associated with the database entry.
        /// </summary>
        [JsonPropertyName("dbColor")]
        public string? DbColor { get; init; }

        /// <summary>
        /// Gets the identifier of the database icon associated with this entity.
        /// </summary>
        [JsonPropertyName("dbIconId")]
        public int DbIconId { get; init; }
    }
}
