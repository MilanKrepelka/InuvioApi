using ASOL.Inuvio.Api.Client.Options;

namespace ASOL.Inuvio.Api.Client.UnitTests.Options
{
    /// <summary>
    /// Testy pro třídu <see cref="InuvioApiConnectionOptions"/>
    /// </summary>
    public class InuvioApiConnectionOptionsTests
    {
        [Fact]
        public void Constructor_ShouldInitializePropertiesWithEmptyStrings()
        {
            var options = new InuvioApiConnectionOptions();

            Assert.Equal(string.Empty, options.UserName);
            Assert.Equal(string.Empty, options.Password);
            Assert.Equal(string.Empty, options.ServerUrl);
        }

        [Fact]
        public void Properties_ShouldBeSettable()
        {
            var options = new InuvioApiConnectionOptions
            {
                UserName = "testuser",
                Password = "testpassword",
                ServerUrl = "https://api.inuvio.com"
            };

            Assert.Equal("testuser", options.UserName);
            Assert.Equal("testpassword", options.Password);
            Assert.Equal("https://api.inuvio.com", options.ServerUrl);
        }

        [Fact]
        public void ServerUrlWithoutTrailingSlash_ShouldRemoveTrailingSlash()
        {
            var options = new InuvioApiConnectionOptions
            {
                ServerUrl = "https://api.inuvio.com/"
            };

            Assert.Equal("https://api.inuvio.com", options.ServerUrlWithoutTrailingSlash);
        }

        [Fact]
        public void ServerUrlWithoutTrailingSlash_ShouldReturnSameUrlWhenNoTrailingSlash()
        {
            var options = new InuvioApiConnectionOptions
            {
                ServerUrl = "https://api.inuvio.com"
            };

            Assert.Equal("https://api.inuvio.com", options.ServerUrlWithoutTrailingSlash);
        }

        [Fact]
        public void ServerUrlWithoutTrailingSlash_ShouldRemoveMultipleTrailingSlashes()
        {
            var options = new InuvioApiConnectionOptions
            {
                ServerUrl = "https://api.inuvio.com///"
            };

            Assert.Equal("https://api.inuvio.com", options.ServerUrlWithoutTrailingSlash);
        }

        [Fact]
        public void ToString_ShouldMaskPassword()
        {
            var options = new InuvioApiConnectionOptions
            {
                ServerUrl = "https://api.inuvio.com",
                UserName = "testuser",
                Password = "secretpassword"
            };

            var result = options.ToString();

            Assert.Contains("ServerUrl:https://api.inuvio.com", result);
            Assert.Contains("UserName:testuser", result);
            Assert.Contains("Password:**??**", result);
            Assert.DoesNotContain("secretpassword", result);
        }

        [Theory]
        [InlineData("https://api.inuvio.com:8080", "https://api.inuvio.com", "8080")]
        [InlineData("http://localhost:3000", "http://localhost", "3000")]
        [InlineData("https://api.inuvio.com", "https://api.inuvio.com", "")]
        [InlineData("api.inuvio.com:443", "api.inuvio.com", "443")]
        [InlineData("", "", "")]
        public void ParseServerUrl_ShouldParseCorrectly(string serverUrl, string expectedUrlWoPort, string expectedPort)
        {
            var (actualUrlWoPort, actualPort) = InuvioApiConnectionOptions.ParseServerUrl(serverUrl);

            Assert.Equal(expectedUrlWoPort, actualUrlWoPort);
            Assert.Equal(expectedPort, actualPort);
        }

        [Fact]
        public void ParseServerUrl_WithHttpsPort_ShouldNotConsiderPortAsPort()
        {
            var (urlWoPort, port) = InuvioApiConnectionOptions.ParseServerUrl("https://api.inuvio.com");

            Assert.Equal("https://api.inuvio.com", urlWoPort);
            Assert.Equal(string.Empty, port);
        }

        [Fact]
        public void ParseServerUrl_WithInvalidPort_ShouldReturnEmptyPort()
        {
            var (urlWoPort, port) = InuvioApiConnectionOptions.ParseServerUrl("https://api.inuvio.com:abc");

            Assert.Equal("https://api.inuvio.com:abc", urlWoPort);
            Assert.Equal(string.Empty, port);
        }

        [Fact]
        public void ParseServerUrl_WithNull_ShouldReturnEmptyStrings()
        {
            var (urlWoPort, port) = InuvioApiConnectionOptions.ParseServerUrl(null!);

            Assert.Equal(string.Empty, urlWoPort);
            Assert.Equal(string.Empty, port);
        }

        [Theory]
        [InlineData("DOMAIN\\user", "DOMAIN", "user")]
        [InlineData("user", "", "user")]
        [InlineData("COMPANY\\john.doe", "COMPANY", "john.doe")]
        [InlineData("", "", "")]
        public void ParseLoginName_ShouldParseCorrectly(string loginName, string expectedDomain, string expectedUser)
        {
            var (actualDomain, actualUser) = InuvioApiConnectionOptions.ParseLoginName(loginName);

            Assert.Equal(expectedDomain, actualDomain);
            Assert.Equal(expectedUser, actualUser);
        }

        [Fact]
        public void ParseLoginName_WithMultipleBackslashes_ShouldUseFirstBackslash()
        {
            var (domain, user) = InuvioApiConnectionOptions.ParseLoginName("DOMAIN\\SUB\\user");

            Assert.Equal("DOMAIN", domain);
            Assert.Equal("SUB\\user", user);
        }

        [Fact]
        public void ParseLoginName_WithNull_ShouldReturnEmptyStrings()
        {
            var (domain, user) = InuvioApiConnectionOptions.ParseLoginName(null!);

            Assert.Equal(string.Empty, domain);
            Assert.Equal(string.Empty, user);
        }
    }
}
