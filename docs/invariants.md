# Invariants

## Implemented Contracts

| Contract | Evidence | Modification consequence |
|---|---|---|
| One update invocation from Main | [Program](../DynamicDnsUpdater.Client/Program.cs) | Adding a loop changes scheduling, resource, and retention assumptions |
| Alias priority is declaration order, not token order | [Input settings](../DynamicDnsUpdater.Client/Configuration/InputSettings.cs), [tests](../DynamicDnsUpdater.Client.UnitTests/Configuration/InputSettingsTests.cs) | Reordering arrays changes public input semantics |
| Last duplicate value wins within one alias | Pinned parser plus [tests](../DynamicDnsUpdater.Client.UnitTests/Configuration/InputSettingsTests.cs) | Parser upgrades require compatibility checks |
| Nonblank selected values remain verbatim | [Input settings](../DynamicDnsUpdater.Client/Configuration/InputSettings.cs) | Adding trimming/case normalization is a behavioural change |
| Request fields are JSON `ip` and `provider` | [DTO](../DynamicDnsUpdater.Client/Requests/PutDnsRecordRequest.cs), [tests](../DynamicDnsUpdater.Client.UnitTests/Requests/PutDnsRecordRequestTests.cs) | Server contract changes require coordination |
| Domain is in endpoint path, not body | [Service](../DynamicDnsUpdater.Client/DnsRecordService.cs) | URL-safety and route compatibility must be considered |
| Discovery precedes service try | [Service](../DynamicDnsUpdater.Client/DnsRecordService.cs) | Moving it changes logging and exit behaviour |
| Caught request failures do not rethrow | [Service](../DynamicDnsUpdater.Client/DnsRecordService.cs) | Adding status propagation changes automation contract |
| No local DNS state authorizes success | [Service](../DynamicDnsUpdater.Client/DnsRecordService.cs) | Do not equate logs with resolver consistency |
| Source settings copy into build output | [Project](../DynamicDnsUpdater.Client/DynamicDnsUpdater.Client.csproj) | Secret provisioning must account for overwrites |

## Important Non-Guarantees

These are not implemented invariants: valid domain syntax, supported provider, required nonempty input, HTTPS-only API, automatic retries, failure exit status for every error, exact-once update, idempotent replay, IPv6 discovery, no external probes, no persistent personal data, or DNS propagation on Success.

The whole interaction preserves submission context: the selected domain/provider and discovered address determine endpoint/body and later logs. It does not preserve an atomic relationship between local logs and remote record state. See [Flow](./flows/dns-update.md), [Testing](./testing.md), and [Change Guide](./change-guide.md).