using System;

using Microsoft.Extensions.DependencyInjection;

using DynamicDnsUpdater.Client.Configuration;
using DynamicDnsUpdater.Client.Service;

namespace DynamicDnsUpdater.Client
{
    public class Program
    {
        public static IServiceProvider ServiceProvider;

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            InputSettings inputSettings = new(args);

            BuildServiceProvider();

            ServiceProvider
                .GetRequiredService<IDomainRecordService>()
                .Update(inputSettings.DomainName, inputSettings.ProviderName);
        }

        static void BuildServiceProvider()
            => ServiceProvider = new ServiceCollection()
                .AddSingleton<IDomainRecordService, DomainRecordService>()
                .BuildServiceProvider();
    }
}
