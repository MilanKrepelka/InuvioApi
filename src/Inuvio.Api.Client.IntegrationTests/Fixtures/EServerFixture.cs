using ASOL.Inuvio.Api.Client.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace ASOL.Inuvio.Api.Client.IntegrationTests.Fixtures
{
    /// <summary>
    /// Fixture for connection to Inuvio API server
    /// </summary>
    public class EServerFixture : TestBedFixture
    {
        protected override ILoggingBuilder AddLoggingProvider(ILoggingBuilder loggingBuilder, ILoggerProvider loggerProvider)
        {
            return base.AddLoggingProvider(loggingBuilder, loggerProvider);
        }

        protected override void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddInuvioApiClient(configuration);
            
        }

        protected override void AddUserSecrets(IConfigurationBuilder configurationBuilder)
        {
            base.AddUserSecrets(configurationBuilder);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override ValueTask DisposeAsyncCore()
        {
            return new ValueTask();
        }

        protected override IEnumerable<TestAppSettings> GetTestAppSettings()
        {
            List<TestAppSettings> testAppSettings = new List<TestAppSettings>();
            testAppSettings.Add(new TestAppSettings()
            {
                Filename = "appsettings.eserver.json"
            });

            return testAppSettings;
        }
    }
}
