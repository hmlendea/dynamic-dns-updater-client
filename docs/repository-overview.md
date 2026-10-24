# Repository Overview

## Purpose And Scope

The client lets an operator request a DNS update for one domain and one provider using the machine's discovered public IPv4 address. [Program](../DynamicDnsUpdater.Client/Program.cs) invokes `DynamicDnsUpdater.Client.IDnsRecordService.Update` once and awaits completion. There is no watch loop or comparison with a previous address.

The selected provider is a string sent to the API, not a locally loaded provider implementation. No DNS-provider SDK, provider credential store, database, queue, web listener, or graphical interface is implemented here. Actual DNS changes and their propagation depend on the remote server and provider.

## Principal Boundaries

| Boundary | Input | Effect | Owner |
|---|---|---|---|
| CLI parsing | Option/value tokens | Select domain and provider strings | [Input settings](./components/input-settings.md) plus NuciCLI.Arguments |
| Composition | Mandatory JSON configuration | Instantiate API client, logger, and update service | [Host](./components/host-and-composition.md) |
| Public-IP discovery | Host network environment | Obtain normalized IPv4 string; may contact several external services | NuciWeb.HTTP |
| Update request | Domain, provider, address, API settings | One JSON PUT with bearer credentials and client metadata | [DNS service](./components/dns-record-service.md) plus NuciAPI.Client |
| DNS mutation | Submitted update | Validation, provider access, record mutation | External API; not inspected here |
| Observability | Operation state and metadata | Console output and default file append | NuciLog |

## Entry Points And Dependants

`DynamicDnsUpdater.Client.Program.Main` is the executable entry. [The solution](../DynamicDnsUpdater.Client.slnx) also includes [the unit-test project](../DynamicDnsUpdater.Client.UnitTests/DynamicDnsUpdater.Client.UnitTests.csproj), which references production code. CI builds and tests this same solution. External automation may invoke the executable, but no scheduler configuration is checked in.

Read [Architecture](./architecture.md) for the global model and [DNS Update Flow](./flows/dns-update.md) for the causal sequence. Crucially, a normally completed process is not sufficient evidence of an API update: exceptions inside the service's request block are logged and swallowed.