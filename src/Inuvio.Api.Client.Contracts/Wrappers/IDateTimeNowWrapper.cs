namespace ASOL.Inuvio.Api.Client.Contracts.Wrappers
{
    /// <summary>
    /// Provides an abstraction for retrieving the current date and time.
    /// </summary>
    /// <remarks>This interface enables decoupling from the static DateTime.UtcNow property, which is useful for
    /// testing and scenarios where the current time needs to be controlled or substituted.</remarks>
    public interface IDateTimeNowWrapper
    {
        /// <summary>
        /// Gets the current date and time in Coordinated Universal Time (UTC).
        /// </summary>
        DateTime UtcNow { get; }
    }
}
