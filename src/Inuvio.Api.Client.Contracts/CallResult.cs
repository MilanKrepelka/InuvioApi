namespace ASOL.Inuvio.Api.Client.Contracts
{
    /// <summary>
    /// Represents the result of an operation, including success status and error information.
    /// </summary>
    public sealed class CallResult : ICallResult
    {
        /// <inheritdoc />
        public bool IsSuccess { get; init; }

        /// <inheritdoc />
        public string ErrorMessage { get; init; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallResult"/> class.
        /// </summary>
        public CallResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallResult"/> class.
        /// </summary>
        /// <param name="isSuccess">Indicates whether the operation was successful.</param>
        /// <param name="errorMessage">The error message if the operation failed.</param>
        public CallResult(bool isSuccess, string errorMessage = "")
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage ?? string.Empty;
        }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        /// <returns>A successful <see cref="CallResult"/>.</returns>
        public static CallResult Success() => new(true);

        /// <summary>
        /// Creates a failed result with the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>A failed <see cref="CallResult"/>.</returns>
        public static CallResult Failure(string errorMessage) => new(false, errorMessage);
    }
}