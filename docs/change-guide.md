# Change Guide

## Impact Map

| Change | Implementation and evidence | Required review |
|---|---|---|
| CLI alias/selection/validation | [InputSettings](../DynamicDnsUpdater.Client/Configuration/InputSettings.cs), [configuration fixtures](../DynamicDnsUpdater.Client.UnitTests/Configuration/) | Parser-owned grammar, precedence, blank/duplicate behavior; [CLI docs](./behaviour/command-line.md), [Configuration](./configuration.md), README examples |
| Startup or configuration source | [Program](../DynamicDnsUpdater.Client/Program.cs), [JSON](../DynamicDnsUpdater.Client/appsettings.json) | Precedence, options lifetime, file resolution, output copies, startup failures; [Host](./components/host-and-composition.md) |
| Endpoint/body/auth | [Service](../DynamicDnsUpdater.Client/DnsRecordService.cs), [DTO](../DynamicDnsUpdater.Client/Requests/PutDnsRecordRequest.cs) | External server contract, URL encoding, package serializer, bearer metadata; [API examples](./api-usage-examples.md), [Integrations](./integrations.md) |
| Result/exit semantics | [Service](../DynamicDnsUpdater.Client/DnsRecordService.cs), [Main](../DynamicDnsUpdater.Client/Program.cs) | Catch placement, log failures, callers/automation; [Errors](./error-handling.md), [Flow](./flows/dns-update.md) |
| Public-IP policy | Static discovery call and package version in [project](../DynamicDnsUpdater.Client/DynamicDnsUpdater.Client.csproj) | IPv4-only behavior, external egress, cache, failure timing, deterministic test seam |
| Log schema/settings | [Keys](../DynamicDnsUpdater.Client/Logging/MyLogInfoKey.cs), [operation](../DynamicDnsUpdater.Client/Logging/MyOperation.cs), [settings](../DynamicDnsUpdater.Client/appsettings.json) | Redaction, fields, sinks, retention, privacy policy; [Logging](./logging.md) |
| Runtime/dependencies/releases | [Projects](../DynamicDnsUpdater.Client.slnx), [workflows](../.github/workflows/), [wrapper](../release.sh) | Exact-tag upstream behavior, package contents, release permissions; [Build](./build-and-deployment.md), [Dependencies](./dependencies.md) |
| Long-running/batch mode | [Main](../DynamicDnsUpdater.Client/Program.cs), [service interface](../DynamicDnsUpdater.Client/IDnsRecordService.cs) | Static state, disposal, concurrent log writes, cache, cancellation, retries, exit aggregation |

## Safe Sequence

1. Identify the owning boundary and the current observed contract; do not substitute generic CLI/HTTP assumptions.
2. Capture the narrow build/test baseline. For uncovered service behavior, first derive cases from [Testing](./testing.md).
3. Make the smallest sufficient change; coordinate intentional breaking wire or CLI changes.
4. Validate the affected tests and solution build; never use a production DNS update as a unit test.
5. Update the relevant flow, component, cross-cutting document, [index](./INDEX.md), and affected root-document sections. Keep policies at root.

No production changes are implied by this guide. Preserve discovered behavior until an intentional behavior change is requested.