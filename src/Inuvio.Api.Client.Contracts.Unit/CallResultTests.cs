

using ASOL.Inuvio.Api.Client.Contracts;

namespace ASOL.Inuvio.Api.Client.Contracts.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="CallResult"/> class.
    /// </summary>
    public sealed class CallResultTests
    {
        /// <summary>
        /// Tests that the default parameterless constructor initializes the CallResult with default values.
        /// Input: None (parameterless constructor).
        /// Expected: IsSuccess is false and ErrorMessage is an empty string.
        /// </summary>
        [Fact]
        public void CallResult_DefaultConstructor_InitializesWithDefaultValues()
        {
            // Arrange & Act
            var result = new CallResult();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(string.Empty, result.ErrorMessage);
        }

        /// <summary>
        /// Tests that the Success method returns a CallResult with IsSuccess set to true
        /// and an empty ErrorMessage.
        /// </summary>
        [Fact]
        public void Success_Always_ReturnsCallResultWithIsSuccessTrueAndEmptyErrorMessage()
        {
            // Arrange
            // No arrangement needed for parameterless static factory method

            // Act
            CallResult result = CallResult.Success();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(string.Empty, result.ErrorMessage);
        }

        /// <summary>
        /// Verifies that the Failure method creates a CallResult with IsSuccess set to false
        /// and ErrorMessage set to the provided message (or empty string if null).
        /// </summary>
        /// <param name="errorMessage">The error message to test.</param>
        /// <param name="expectedErrorMessage">The expected error message after processing.</param>
        [Theory]
        [InlineData("An error occurred", "An error occurred")]
        [InlineData("", "")]
#pragma warning disable xUnit1012 // Null should only be used for nullable parameters
        [InlineData(null, "")]
#pragma warning restore xUnit1012 // Null should only be used for nullable parameters
        [InlineData("   ", "   ")]
        [InlineData("\t\n", "\t\n")]
        [InlineData("Error with special chars: !@#$%^&*()", "Error with special chars: !@#$%^&*()")]
        public void Failure_WithVariousErrorMessages_ReturnsFailedResultWithCorrectErrorMessage(string errorMessage, string expectedErrorMessage)
        {
            // Act
            var result = CallResult.Failure(errorMessage);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedErrorMessage, result.ErrorMessage);
        }

        /// <summary>
        /// Verifies that the Failure method handles very long error messages correctly.
        /// </summary>
        [Fact]
        public void Failure_WithVeryLongErrorMessage_ReturnsFailedResultWithFullMessage()
        {
            // Arrange
            var longErrorMessage = new string('x', 10000);

            // Act
            var result = CallResult.Failure(longErrorMessage);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(longErrorMessage, result.ErrorMessage);
        }

        /// <summary>
        /// Verifies that the Failure method handles error messages with Unicode characters correctly.
        /// </summary>
        [Fact]
        public void Failure_WithUnicodeCharacters_ReturnsFailedResultWithUnicodeMessage()
        {
            // Arrange
            var unicodeMessage = "Error: 你好世界 🚀 Ошибка";

            // Act
            var result = CallResult.Failure(unicodeMessage);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(unicodeMessage, result.ErrorMessage);
        }

        /// <summary>
        /// Tests that the constructor correctly sets IsSuccess property with the provided boolean value
        /// and ErrorMessage property with the provided error message value.
        /// </summary>
        /// <param name="isSuccess">The success status to pass to the constructor.</param>
        /// <param name="errorMessage">The error message to pass to the constructor.</param>
        /// <param name="expectedErrorMessage">The expected error message after constructor execution.</param>
        [Theory]
        [InlineData(true, "Error occurred", "Error occurred")]
        [InlineData(false, "Error occurred", "Error occurred")]
        [InlineData(true, "", "")]
        [InlineData(false, "", "")]
        [InlineData(true, "   ", "   ")]
        [InlineData(false, "   ", "   ")]
        [InlineData(true, "Very long error message with many characters to test edge case behavior when strings are extremely long and contain lots of text", "Very long error message with many characters to test edge case behavior when strings are extremely long and contain lots of text")]
        [InlineData(false, "Error with special chars: !@#$%^&*()_+-=[]{}|;:',.<>?/~`", "Error with special chars: !@#$%^&*()_+-=[]{}|;:',.<>?/~`")]
        public void Constructor_WithIsSuccessAndErrorMessage_SetsPropertiesCorrectly(bool isSuccess, string errorMessage, string expectedErrorMessage)
        {
            // Arrange & Act
            var result = new CallResult(isSuccess, errorMessage);

            // Assert
            Assert.Equal(isSuccess, result.IsSuccess);
            Assert.Equal(expectedErrorMessage, result.ErrorMessage);
        }

        /// <summary>
        /// Tests that the constructor correctly handles null error message by converting it to an empty string.
        /// </summary>
        /// <param name="isSuccess">The success status to pass to the constructor.</param>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Constructor_WithNullErrorMessage_SetsErrorMessageToEmpty(bool isSuccess)
        {
            // Arrange & Act
            var result = new CallResult(isSuccess, null!);

            // Assert
            Assert.Equal(isSuccess, result.IsSuccess);
            Assert.Equal(string.Empty, result.ErrorMessage);
        }

        /// <summary>
        /// Tests that the constructor uses default empty string for errorMessage when not provided.
        /// </summary>
        /// <param name="isSuccess">The success status to pass to the constructor.</param>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Constructor_WithDefaultErrorMessage_SetsErrorMessageToEmpty(bool isSuccess)
        {
            // Arrange & Act
            var result = new CallResult(isSuccess);

            // Assert
            Assert.Equal(isSuccess, result.IsSuccess);
            Assert.Equal(string.Empty, result.ErrorMessage);
        }

        /// <summary>
        /// Tests that the constructor correctly handles error messages containing newlines and control characters.
        /// </summary>
        /// <param name="isSuccess">The success status to pass to the constructor.</param>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Constructor_WithErrorMessageContainingNewlinesAndControlChars_SetsErrorMessageCorrectly(bool isSuccess)
        {
            // Arrange
            var errorMessage = "Error\nwith\nnewlines\rand\ttabs";

            // Act
            var result = new CallResult(isSuccess, errorMessage);

            // Assert
            Assert.Equal(isSuccess, result.IsSuccess);
            Assert.Equal(errorMessage, result.ErrorMessage);
        }

        /// <summary>
        /// Tests that the constructor correctly handles Unicode and non-ASCII characters in error messages.
        /// </summary>
        /// <param name="isSuccess">The success status to pass to the constructor.</param>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Constructor_WithUnicodeCharactersInErrorMessage_SetsErrorMessageCorrectly(bool isSuccess)
        {
            // Arrange
            var errorMessage = "Error 错误 エラー خطأ 🚨";

            // Act
            var result = new CallResult(isSuccess, errorMessage);

            // Assert
            Assert.Equal(isSuccess, result.IsSuccess);
            Assert.Equal(errorMessage, result.ErrorMessage);
        }
    }
}
