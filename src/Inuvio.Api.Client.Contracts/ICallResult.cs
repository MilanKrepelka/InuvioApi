namespace ASOL.Inuvio.Api.Client.Contracts
{
    /// <summary>
    /// Interface for operation result objects, providing success status and error information.
    /// </summary>
    public interface ICallResult
    {
        /// <summary>
        /// Gets a value indicating whether the operation completed successfully.
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Gets the error message associated with the operation, if any.
        /// </summary>
        string ErrorMessage { get; }
    }
}