# State And Persistence

## State Inventory

| State | Owner | Lifetime and mutation |
|---|---|---|
| Parsed domain/provider | Application input object | One invocation; immutable properties |
| API settings/key | Local JSON and options object | File persists; bound values retained in process |
| DI provider and input reference | Static fields in Program | Process lifetime; not explicitly disposed/reset |
| API base URL and HTTP client | NuciAPI.Client singleton | Base URL captured at construction; HTTP client reused in process |
| Discovered address | Request object and NuciWeb.HTTP cache | Request lifetime; package cache expires after two minutes |
| Operation fields | Local enumerable in update method | Appended as execution advances; not a durable transaction |
| Logs | NuciLog console and file sink | File appends persist until operator rotation/deletion |
| DNS records | External API/provider | Remote authoritative state; not read back locally |

Sources: [Program](../DynamicDnsUpdater.Client/Program.cs), [service](../DynamicDnsUpdater.Client/DnsRecordService.cs), [settings](../DynamicDnsUpdater.Client/appsettings.json), and [version-pinned dependencies](./dependencies.md).

## Consistency And Recovery

No database, local record cache, journal, queue, migration, checkpoint, or last-success marker exists. The package's address cache is not persisted across executable runs and does not suppress update submissions. Logs are observational evidence, not a commit record: server mutation can succeed before response delivery fails, and Success logging can itself fail.

Restarting loses process state and triggers a new discovery/update path. Repeated invocations can append to the same log file. Rebuilds copy source configuration to output, so output-only secret edits may be overwritten. Protect all configuration copies and rotate logs externally.

The published [Privacy Policy](../PRIVACY.md) describes no local file logging, but the shipped configuration enables it. Treat that as a recorded policy/code discrepancy, not evidence that log persistence is absent; see [Open Questions](./ambiguities-and-open-questions.md).