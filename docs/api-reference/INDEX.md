# API Reference

## Scope

This repository is an HTTP client, not an API host. There are no controllers or controller actions, inbound authentication schemes, exposed API versions, server-side rate-limit policies, or inbound response generators to document. Accordingly no controller files are invented. The following is the complete outbound endpoint inventory from [the service](../../DynamicDnsUpdater.Client/DnsRecordService.cs).

| Method | Path relative to configured base URL | Controller | Action/caller | Auth |
|---|---|---|---|---|
| PUT | `DnsRecords/{domainName}` | External; unknown | `DynamicDnsUpdater.Client.DnsRecordService.Update` | Bearer token when configured |

No route version is added locally; any API prefix/version must be part of the base URL. Server rate limits, supported providers, authorization policies, status-code catalogue, and response guarantees are not defined in this repository. No client retry/backoff handles throttling.

## Outbound Contract

- Path input: domain string, interpolated without explicit escaping or local constraints; no query parameters are added by this call.
- Body: JSON `ip` and `provider` strings, without local semantic validation.
- Headers: JSON UTF-8 content type; optional bearer Authorization; package-generated client ID, request ID, and timestamp.
- Responses: package success/error envelopes, selected by HTTP status; malformed content throws into the service catch.
- Side effect: asks an external server to update DNS; idempotency and propagation are server-owned and unverified.

See [API Examples](../api-usage-examples.md) for illustrative messages, [Integrations](../integrations.md) for package mechanics, [DNS Update Flow](../flows/dns-update.md) for failures, [Service Component](../components/dns-record-service.md) for implementation ownership, and [Data Model](../data-model.md) for schema details. Endpoint-specific server errors cannot be supplied as verified examples without inspecting the server repository.