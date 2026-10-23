using NuciCLI.Arguments;

namespace DynamicDnsUpdater.Client.Configuration
{
    public sealed class InputSettings
    {
        static readonly string[] DomainNameOptions = ["-d", "--dn", "--domain", "--domain-name"];
        static readonly string[] ProviderNameOptions = ["-p", "--pn", "--provider", "--provider-name"];

        public string DomainName { get; }
        public string ProviderName { get; }

        public InputSettings(string[] args)
        {
            ArgumentsCollection parsedArgs = ParseArguments(args);

            DomainName = GetOptionValue(parsedArgs, DomainNameOptions);
            ProviderName = GetOptionValue(parsedArgs, ProviderNameOptions);
        }

        static ArgumentsCollection ParseArguments(string[] args)
        {
            ArgumentParser argumentParser = new();

            foreach (string option in DomainNameOptions)
            {
                argumentParser.AddArgument(NormalizeOptionName(option), string.Empty, required: false, defaultValue: string.Empty);
            }

            foreach (string option in ProviderNameOptions)
            {
                argumentParser.AddArgument(NormalizeOptionName(option), string.Empty, required: false, defaultValue: string.Empty);
            }

            return argumentParser.ParseArgs(args);
        }

        static string GetOptionValue(ArgumentsCollection arguments, string[] options)
        {
            foreach (string option in options)
            {
                string optionName = NormalizeOptionName(option);

                if (arguments.Has(optionName))
                {
                    string value = arguments.Get<string>(optionName);

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        return value;
                    }
                }
            }

            return string.Empty;
        }

        static string NormalizeOptionName(string option)
            => option.TrimStart('-');
    }
}
