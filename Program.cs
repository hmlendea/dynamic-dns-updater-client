using System;

using Microsoft.Extensions.DependencyInjection;

using DynamicDnsUpdater.Client.Configuration;
using Microsoft.Extensions.Configuration;
using NuciAPI.Client;
using Microsoft.Extensions.Options;
using NuciLog.Core;
using NuciLog;
using NuciLog.Configuration;
using System.Threading.Tasks;

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
        public static async Task Main(string[] args)
        {
            inputSettings = new(args);

            BuildServiceProvider();

            await ServiceProvider
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
                .Configure<ApiSettings>(configuration.GetSection(nameof(ApiSettings)))
                .Configure<NuciLoggerSettings>(configuration.GetSection(nameof(NuciLoggerSettings)))
                .AddSingleton(provider => provider.GetRequiredService<IOptions<NuciLoggerSettings>>().Value)
                .AddTransient<IDnsRecordService, DnsRecordService>()
                .AddSingleton<INuciApiClient>(provider => new NuciApiClient(provider.GetRequiredService<IOptions<ApiSettings>>().Value.BaseUrl))
                .AddTransient<ILogger, NuciLogger>()
                .BuildServiceProvider();
        }
    }
}
