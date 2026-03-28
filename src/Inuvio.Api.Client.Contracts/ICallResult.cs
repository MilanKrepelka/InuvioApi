namespace ASOL.Inuvio.Api.Client.Contracts
{

    /// <summary>
    /// Defines the result of an operation, including success status and error information.
    /// </summary>
    /// <remarks>Implementations of this interface provide a standardized way to represent the outcome of an
    /// operation. Use the properties to determine whether the operation succeeded and to retrieve error details if it
    /// did not.</remarks>
    public interface ICallResult
    {
        /// <summary>
        /// Gets a value indicating whether the operation completed successfully.
        /// </summary>
        bool IsSuccess { get; init; }
        /// <summary>
        /// Gets the error message associated with the current operation or state.
        /// </summary>
        string ErrorMessage { get; init; }
    }
}
