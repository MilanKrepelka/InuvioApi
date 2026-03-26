using System;
using System.Collections.Generic;
using System.Text;

namespace ASOL.Inuvio.Api.Client.Wrappers
{
    /// <summary>
    /// Provides an abstraction for retrieving the current date and time.
    /// </summary>
    /// <remarks>This interface enables decoupling from the static DateTime.UtcNow property, which is useful for
    /// testing and scenarios where the current time needs to be controlled or substituted.</remarks>
    public interface IDateTimeNowWrapper
    {
        DateTime UtcNow { get; }
    }
}
