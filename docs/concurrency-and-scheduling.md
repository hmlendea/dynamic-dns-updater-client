# Concurrency And Scheduling

## Application Execution

`DynamicDnsUpdater.Client.Program.Main` awaits exactly one update. `DynamicDnsUpdater.Client.DnsRecordService.Update` does synchronous metadata/logging/discovery work before awaiting HTTP. There is no application-created task fan-out, timer, worker loop, lock, cancellation token, or scheduler.

Public-IP discovery is a synchronous package API backed by blocking waits on async network work. NuciWeb.HTTP 1.7.3 performs concurrent connectivity probes internally, then attempts shuffled address services. It uses a shared HTTP client and thread-safe process-local cache. This internal concurrency does not make the application a background service.

## Resource And Ordering Implications

Started logging precedes discovery; a slow lookup blocks progression before the API request and before the catch boundary. File logging uses synchronous append. No application-level end-to-end deadline is configured. HTTP/client defaults and discovery's internal probe timeouts are package-owned; adding a scheduler does not add a bounded runtime.

If an operator starts overlapping processes, each has its own cache and container, may submit competing updates, and may append to the same file. There is no distributed lock or record-level ordering. If callers reuse the service concurrently in a future host, singleton API-client behavior, static discovery caches, options snapshots, and unsynchronized file writes require review.

## Scheduling Contract

Use external automation for repetition; no cron expression, systemd unit, Windows scheduled task, container schedule, or daemon configuration is checked in. Do not mistake [the editor watch task](../.vscode/tasks.json) for production scheduling: it reruns development code on changes.

Evidence: [entry point](../DynamicDnsUpdater.Client/Program.cs), [service](../DynamicDnsUpdater.Client/DnsRecordService.cs), and [dependency sources](./dependencies.md). Flow/concurrency and overlapping-process scenarios currently lack tests; see [Testing](./testing.md).