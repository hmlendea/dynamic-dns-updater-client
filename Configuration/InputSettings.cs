using NuciCLI;

namespace DynamicDnsUpdater.Client.Configuration
{
    public sealed class InputSettings(string[] args)
    {
        static readonly string[] DomainNameOptions = ["-d", "--dn", "--domain", "--domain-name"];
        static readonly string[] ProviderNameOptions = ["-p", "--pn", "--provider", "--provider-name"];

        public string DomainName { get; set; } = CliArgumentsReader.GetOptionValue(args, DomainNameOptions);
        public string ProviderName { get; set; } = CliArgumentsReader.GetOptionValue(args, ProviderNameOptions);
    }
}
