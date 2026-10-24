# Configuration

## Sources And Binding

`DynamicDnsUpdater.Client.Program.BuildServiceProvider` in [Program](../DynamicDnsUpdater.Client/Program.cs) adds only a mandatory JSON file named `appsettings.json`, with `reloadOnChange: true`. It does not add environment variables, user secrets, command-line configuration, or environment-specific JSON files. CLI domain/provider values are parsed separately and never override API settings.

The [checked-in JSON](../DynamicDnsUpdater.Client/appsettings.json) uses lower-camel-case section names; Microsoft configuration binding matches the PascalCase section names used by the host case-insensitively. There is no options validation registration.

| Section/key | Type | Shipped value or default | Effective meaning |
|---|---|---|---|
| `apiSettings.baseUrl` | string | `[[DYNAMIC_DNS_UPDATER_API_BASE_URL]]` placeholder | Outbound API URL prefix; no local HTTPS enforcement |
| `apiSettings.apiKey` | string | `[[DYNAMIC_DNS_UPDATER_API_KEY]]` placeholder | Bearer token; empty token causes package client to omit Authorization |
| `nuciLoggerSettings.logFilePath` | string | `logfile.log` | Relative to process working directory unless made absolute |
| `nuciLoggerSettings.isFileOutputEnabled` | bool | `true` | Enable file append; does not disable console output |
| `nuciLoggerSettings.minimumLevel` | package log-level value | `Info` package default | Package filters less-severe messages |
| `nuciLoggerSettings.timestampFormat` | string | `yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK` package default | Local timestamp including offset information |
| `nuciLoggerSettings.logLineFormat` | string | Package default: four format fields separated by a fullwidth vertical line | Timestamp, source context, level, message formatting |

The first four rows are explicit in the shipped JSON. Other logger settings are inherited from NuciLog 1.2.1, verified in [versioned source](https://github.com/hmlendea/nucilog/blob/v1.2.1/NuciLog/Configuration/NuciLoggerSettings.cs). API settings are simple uninitialized string properties in [ApiSettings](../DynamicDnsUpdater.Client/Configuration/ApiSettings.cs), not required validated options.

## CLI Schema And Precedence

| Setting | Accepted aliases, highest priority first | Default |
|---|---|---|
| Domain | `--d`, `--dn`, `--domain`, `--domain-name` | Empty string |
| Provider | `--p`, `--pn`, `--provider`, `--provider-name` | Empty string |

Each option consumes the next token as its value. All options are optional. Repeating the same alias keeps its last value; competing aliases are resolved by the fixed order in this table, not token order. Whitespace-only values are skipped; other strings retain whitespace, case, and invalid-looking content. See [Input Settings](./components/input-settings.md).

## File Resolution And Deployment

There is no explicit `SetBasePath` in the host. Keep the settings file available to the configuration provider and launch from the application/output directory; this also makes relative logging predictable. The debugger explicitly sets [the application directory as its working directory](../.vscode/launch.json). [The project](../DynamicDnsUpdater.Client/DynamicDnsUpdater.Client.csproj) copies the settings file to build output with `CopyToOutputDirectory=Always`; rebuilds can replace output-directory edits.

## Reload Semantics

File watching is enabled, but the application consumes `IOptions<T>.Value`, not `IOptionsMonitor<T>`. API client construction captures the base URL; logger settings are extracted into a singleton. Do not promise effective hot reload. Restart the one-shot invocation after configuration changes. In-process reassignment of `ApiSettings.BaseUrl` could also diverge from the already-constructed client's captured base URL.

## Validation And Secrecy

Missing/malformed JSON fails startup. Missing or placeholder API values are not proactively rejected by application code; failures surface during client construction, HTTP request creation, or authentication. Protect source and copied output settings, backups, and package artefacts. No secret provider or automatic placeholder replacement exists.