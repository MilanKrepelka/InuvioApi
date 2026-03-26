using ASOL.Inuvio.Api.Client.Wrappers;

namespace ASOL.Inuvio.Api.Client.UnitTests.Wrappers
{
    public class DateTimeNowWrapperTests
    {
        [Fact]
        public void UtcNow_ShouldReturnCurrentUtcDateTime()
        {
            var wrapper = new DateTimeNowWrapper();
            var beforeCall = DateTime.UtcNow;

            var result = wrapper.UtcNow;

            var afterCall = DateTime.UtcNow;

            Assert.InRange(result, beforeCall.AddMilliseconds(-100), afterCall.AddMilliseconds(100));
        }

        [Fact]
        public void UtcNow_ShouldReturnUtcKind()
        {
            var wrapper = new DateTimeNowWrapper();

            var result = wrapper.UtcNow;

            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        [Fact]
        public void UtcNow_CalledMultipleTimes_ShouldReturnDifferentOrEqualValues()
        {
            var wrapper = new DateTimeNowWrapper();

            var firstCall = wrapper.UtcNow;
            Thread.Sleep(10);
            var secondCall = wrapper.UtcNow;

            Assert.True(secondCall >= firstCall);
        }

        [Fact]
        public void UtcNow_ShouldBeCloseToSystemUtcNow()
        {
            var wrapper = new DateTimeNowWrapper();

            var wrapperTime = wrapper.UtcNow;
            var systemTime = DateTime.UtcNow;

            var difference = Math.Abs((wrapperTime - systemTime).TotalMilliseconds);
            Assert.True(difference < 100, $"Difference was {difference}ms, expected less than 100ms");
        }

        [Fact]
        public void UtcNow_ShouldImplementIDateTimeNowWrapper()
        {
            var wrapper = new DateTimeNowWrapper();

            Assert.IsAssignableFrom<IDateTimeNowWrapper>(wrapper);
        }
    }
}
