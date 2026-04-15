using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using NuciAPI.Client;
using NuciLog.Core;
using NuciLog;
using NuciLog.Configuration;

using DynamicDnsUpdater.Client.Configuration;

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
