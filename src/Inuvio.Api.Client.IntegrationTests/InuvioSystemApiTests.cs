using ASOL.Inuvio.Api.Client;
using ASOL.Inuvio.Api.Client.Helpers;
using ASOL.Inuvio.Api.Client.Options;
using Castle.Core.Logging;
using ASOL.Inuvio.Api.Client.Contracts;
using ASOL.Inuvio.Api.Client.Extensions;

using ASOL.Inuvio.Api.Client.Wrappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using ASOL.Inuvio.Api.Client.IntegrationTests.Fixtures;

namespace ASOL.Inuvio.Api.Client.IntegrationTests
{
    public class InuvioSystemApiTests : TestBed<EServerFixture>
    {
        public InuvioSystemApiTests(ITestOutputHelper testOutputHelper, EServerFixture fixture) : base(testOutputHelper, fixture)
        {
        }

        [Fact]
        public async Task GetStatusAsync_Returns_ValidStatus()
        {
            var inuvioSystemApi = _fixture.GetServiceProvider(_testOutputHelper).GetRequiredService<IInuvioSystemApi>();

            var result = await inuvioSystemApi.GetStatusAsync(CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotNull(result.Licence);
            Assert.NotNull(result.SqlServer);
            Assert.NotNull(result.Server);
            Assert.NotNull(result.IdCode);
            Assert.NotNull(result.Version);
        }

        [Fact]
        public async Task GetDatabasesAsync_Returns_ValidDatabases()
        {
            var inuvioSystemApi = _fixture.GetServiceProvider(_testOutputHelper).GetRequiredService<IInuvioSystemApi>();

            var result = await inuvioSystemApi.GetDatabasesAsync(CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Databases);
        }
    }
}
