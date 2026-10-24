# Host And Composition

## Purpose And Entry

[Program](../../DynamicDnsUpdater.Client/Program.cs) owns executable startup, not DNS rules. `DynamicDnsUpdater.Client.Program.Main` constructs input settings before reading configuration, calls its private composition method, resolves one service, and awaits `DynamicDnsUpdater.Client.IDnsRecordService.Update` with the selected strings. There is no generic host, middleware, hosted service, or top-level catch.

## Container Graph

| Registration | Lifetime | Consumer and consequence |
|---|---|---|
| `Microsoft.Extensions.Configuration.IConfiguration` | Singleton | Configuration/options binding |
| `DynamicDnsUpdater.Client.Configuration.ApiSettings` via options | Options-managed | Service reads credential/base URL; API-client factory captures base URL |
| `NuciLog.Configuration.NuciLoggerSettings` via options and explicit value | Singleton extracted value | Logger construction; not a live monitor |
| `DynamicDnsUpdater.Client.IDnsRecordService` to `DynamicDnsUpdater.Client.DnsRecordService` | Transient | Main resolves once |
| `NuciAPI.Client.INuciApiClient` | Singleton | Constructed from API base URL; package owns HTTP client |
| `NuciLog.Core.ILogger` to `NuciLog.NuciLogger` | Transient | Injected into service |

The public static service-provider field and private static input-settings field retain process-level references. There is no explicit provider disposal; changing this to a long-running host would require lifetime, watcher, and HTTP resource review.

## Failure And Modification Impact

Argument errors precede configuration/logging. JSON errors and dependency-construction failures precede the update service's catch. Adding configuration sources must explicitly define precedence, update [Configuration](../configuration.md), and add startup tests: no composition-root tests currently exist. Replacing the service implementation must preserve the interface but must not assume it returns a success flag: its contract is only `Task`.

See [Startup Flow](../flows/startup.md), [Dependencies](../dependencies.md), and [Errors](../error-handling.md).