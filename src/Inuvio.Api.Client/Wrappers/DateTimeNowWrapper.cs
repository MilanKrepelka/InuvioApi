using System;
using System.Collections.Generic;
using System.Text;

namespace ASOL.Inuvio.Api.Client.Wrappers
{
    /// <summary>
    /// Gets the current date and time in Coordinated Universal Time (UTC).
    /// </summary>
    internal class DateTimeNowWrapper: IDateTimeNowWrapper
    {
        /// <summary>
        /// Gets the current date and time in Coordinated Universal Time (UTC).
        /// </summary>
        public DateTime UtcNow => DateTime.UtcNow;
    
    }
}
