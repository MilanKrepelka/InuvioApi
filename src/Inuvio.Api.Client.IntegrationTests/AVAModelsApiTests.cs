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
    public class AVAModelsApiTests : TestBed<EServerFixture>
    {
        public AVAModelsApiTests(ITestOutputHelper testOutputHelper, EServerFixture fixture) : base(testOutputHelper, fixture)
        {
        }

        [Fact]
        public async Task GetAVAModelsAsync_Returns_AVAModels()
        {
            var avaModelsApi = _fixture.GetServiceProvider(_testOutputHelper).GetRequiredService<IAVAModelsApi>();

            var result = await avaModelsApi.GetAVAModelsAsync(CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Models);
            
        }

    }
}
