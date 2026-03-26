using ASOL.Inuvio.Api.Client.Options;
using ASOL.Inuvio.Api.Client.Contracts;
using ASOL.Inuvio.Api.Client.SystemApi;
using ASOL.Inuvio.Api.Client.Wrappers;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Text.Json;
using ASOL.Inuvio.Api.Client.Handlers;
using Xunit.Microsoft.DependencyInjection.Abstracts;

using ASOL.Inuvio.Api.Client.Extensions;
using ASOL.Inuvio.Api.Client.IntegrationTests.Fixtures;


namespace ASOL.Inuvio.Api.Client.UnitTests.Integration
{
    public class InuvioApiClientTests : TestBed<EServerFixture>
    {
        public InuvioApiClientTests(ITestOutputHelper testOutputHelper, EServerFixture fixture) : base(testOutputHelper, fixture)
        {
        }

        [Fact]
        public async Task CheckConnection_ValidConnection_IsSuccess()
        {
            var inuvioClient = _fixture.GetServiceProvider(_testOutputHelper).GetRequiredService<IInuvioApiClient>();

            var result = await inuvioClient.CheckConnection(CancellationToken.None);

            Assert.True(result.IsSuccess, $"Expected connection to be successful, but got error: {result.ErrorMessage}");
        }


        [Fact]
        public async Task CheckConnection_Create_20_Client_With_Success()
        {
            // Vytvoříme 20 klientů
            var clients = Enumerable.Range(0, 20)
                .Select(_ => _fixture.GetServiceProvider(_testOutputHelper).GetRequiredService<IInuvioApiClient>())
                .ToList();

            // Paralelně zavoláme CheckConnection na všech klientech
            var results = await Task.WhenAll(clients.Select(client => client.CheckConnection(CancellationToken.None)));

            // Ověříme, že všechny výsledky jsou úspěšné
            Assert.All(results, result => 
                Assert.True(result.IsSuccess, $"Expected connection to be successful, but got error: {result.ErrorMessage}")
            );
        }

        [Fact]
        public async Task CheckConnection_InvalidConnection_Failed()
        {
            // Arrange
            var inuvioClient = CreateInuvioClientWithInvalidCredentials();

            // Act
            var result = await inuvioClient.CheckConnection(CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess, "Expected connection to fail with invalid credentials");
            Assert.False(string.IsNullOrEmpty(result.ErrorMessage), "Expected error message to be present");
        }

        private IInuvioApiClient CreateInuvioClientWithInvalidCredentials()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            
           services.AddInuvioApiClient(options => 
           {
               options.ServerUrl = "http://172.29.7.63:8888";
               options.Password = "";
               options.UserName = "";
           });
            
            return services.BuildServiceProvider().GetRequiredService<IInuvioApiClient>();
        }
    }
}
