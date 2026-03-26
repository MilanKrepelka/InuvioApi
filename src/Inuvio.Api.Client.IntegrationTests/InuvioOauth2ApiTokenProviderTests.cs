using ASOL.Inuvio.Api.Client.Contracts;
using ASOL.Inuvio.Api.Client.IntegrationTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace ASOL.Inuvio.Api.Client.IntegrationTests
{
    public class InuvioOauth2ApiTokenProviderTests : TestBed<EServerFixture>
    {
        public InuvioOauth2ApiTokenProviderTests(ITestOutputHelper testOutputHelper, EServerFixture fixture) : base(testOutputHelper, fixture)
        {
        }

        [Fact]
        public async Task GetToken_Returns_ValidToken()
        {
            var tokenProvider = _fixture.GetServiceProvider(_testOutputHelper).GetRequiredService<IInuvioApiTokenProvider>();
            var token = await tokenProvider.GetTokenAsync();
            Assert.NotNull(token);
        }
    }
}
