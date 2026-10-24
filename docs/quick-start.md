# Quick Start

## Prerequisites

Use the .NET 10 SDK and an accessible Dynamic DNS Updater API whose operator has supplied a valid API credential and provider identifier. Build and unit tests do not require production credentials or a live DNS service. A real invocation can modify a real DNS record.

## Build And Test

From the repository root, run `dotnet restore`, `dotnet build DynamicDnsUpdater.Client.slnx`, and `dotnet test DynamicDnsUpdater.Client.slnx --no-build`. These commands follow [CI](../.github/workflows/dotnet.yml).

## Configure A Real Update

Replace the two placeholder strings in [the application settings](../DynamicDnsUpdater.Client/appsettings.json) with your trusted API base URL and credential. Use HTTPS and protect this file; do not commit credentials. The placeholders are literal strings, not environment-variable substitutions. See [Configuration](./configuration.md).

Run from the application directory so the mandatory JSON file is available. After changing into [that directory](../DynamicDnsUpdater.Client/), use `dotnet run -- --domain-name example.com --provider-name cloudflare`, substituting a domain and provider you are authorized to update. Use double-hyphen options: the README's historical single-hyphen examples do not match the pinned parser.

## Verify The Result

Inspect the `UpdateDnsRecord` terminal operation status in console output and the configured log file. A Success log means the client received a successful response type, not that DNS propagation was checked. Confirm the record through the API/provider and the relevant DNS resolution path when operational correctness matters.

A Failure log can coexist with normal process completion. Do not use the exit status alone as a health check. [Troubleshooting](./troubleshooting.md) explains the different failure boundaries.

## Avoid Unintended Network Calls

There is no dry-run, built-in help, or version option. Even empty CLI input proceeds toward update execution. Do not invoke the program as a harmless installation check; build and unit tests are the safe offline checks for application behaviour.