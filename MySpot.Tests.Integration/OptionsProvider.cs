using Microsoft.Extensions.Configuration;
using MySpot.Infrastructure;

namespace MySpot.Tests.Integration
{
    public class OptionsProvider
    {
        private readonly IConfiguration _configuration;

        public OptionsProvider()
        {
            _configuration = GetConfigurationRoot();
        }

        public T Get<T>(string sectionNmae) where T : class, new() 
            => _configuration.GetSection<T>(sectionNmae);

        private static IConfigurationRoot GetConfigurationRoot()
            => new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json", false)
                .AddEnvironmentVariables()
                .Build();
    }
}
