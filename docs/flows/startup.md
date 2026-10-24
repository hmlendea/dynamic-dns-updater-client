# Startup Flow

## Causal Sequence

1. The runtime calls `DynamicDnsUpdater.Client.Program.Main` in [Program](../../DynamicDnsUpdater.Client/Program.cs) with tokenized CLI arguments.
2. `DynamicDnsUpdater.Client.Configuration.InputSettings` registers aliases, delegates parsing, and chooses values. Parse errors escape immediately; configuration and logger have not been built.
3. The composition method loads mandatory JSON configuration and enables file watching. Missing or malformed JSON stops startup.
4. The container registers API/logger options, API client, logger, and DNS service. Actual API-client/logger creation occurs through service resolution.
5. Main resolves `DynamicDnsUpdater.Client.IDnsRecordService`; dependency/options construction failures escape.
6. Main awaits `DynamicDnsUpdater.Client.IDnsRecordService.Update` with domain/provider and returns when it completes. There is no result interpretation, explicit exit-code assignment, or host run loop.

## State And Alternate Paths

Input settings and the service provider are static fields. No explicit disposal occurs. Empty values are a normal path to step 6, not a startup-validation branch. No environment selection or help branch bypasses the update.

[Editor launch settings](../../.vscode/launch.json) supply no arguments and set the application working directory, so default debugging can reach an empty-input update. The editor's watch task is development tooling, not a production scheduling feature.

## Evidence And Next Boundary

Input branch semantics are covered by [the parser fixtures](../../DynamicDnsUpdater.Client.UnitTests/Configuration/); startup composition itself is untested. Continue with [DNS Update Flow](./dns-update.md). Configuration-source changes affect [Configuration](../configuration.md), [Host Composition](../components/host-and-composition.md), and deployment launch assumptions.