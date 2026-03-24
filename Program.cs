using System;

using Microsoft.Extensions.DependencyInjection;

using DynamicDnsUpdater.Client.Configuration;
using Microsoft.Extensions.Configuration;
using NuciAPI.Client;
using Microsoft.Extensions.Options;

namespace DynamicDnsUpdater.Client
{
    public class Program
    {
        public static IServiceProvider ServiceProvider;

        static InputSettings inputSettings;

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            inputSettings = new(args);

            BuildServiceProvider();

            ServiceProvider
                .GetRequiredService<IDnsRecordService>()
                .Update(inputSettings.DomainName, inputSettings.ProviderName);
        }

        static void BuildServiceProvider()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            ServiceProvider = new ServiceCollection()
                .AddSingleton(configuration)
                .Configure<ApiSettings>(configuration.GetSection("apiSettings"))
                .AddTransient<IDnsRecordService, DnsRecordService>()
                .AddTransient<INuciApiClient>(provider => new NuciApiClient(provider.GetRequiredService<IOptions<ApiSettings>>().Value.BaseUrl))
                .BuildServiceProvider();
        }
    }
}
