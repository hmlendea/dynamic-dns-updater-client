# Integrations

## Configured DNS Updater API

[The service](../DynamicDnsUpdater.Client/DnsRecordService.cs) sends PUT to `DnsRecords/{domain}` using the base URL captured when NuciAPI.Client is constructed. Version 1.2.4 concatenates base and endpoint, inserting `/` only when neither boundary already has one. This is string joining, not a validated domain-to-path mapping. The service performs no explicit domain encoding, so slashes, query delimiters, or fragments in input can alter URI interpretation.

For PUT, the package uses its JSON extension serializer and UTF-8 `application/json` content. It adds:
- `Authorization: Bearer ...` only when token is nonempty.
- `X-Client-ID`: URI-escaped `DynamicDnsUpdater.Client.{Environment.MachineName}`.
- `X-Request-ID`: URI-escaped newly generated uppercase GUID.
- `X-Timestamp`: URI-escaped `DateTimeOffset.Now` round-trip (`o`) string.

The application supplies no HMAC shared secret, so the package does not add `X-HMAC`. It does not attach a cancellation token or configure timeouts/retries. Inbound response headers are not examined. Non-2xx dispatches to an error type; 2xx dispatches to the success type. See [DNS Update Flow](./flows/dns-update.md).

Server implementation, provider authentication, allowed provider names, domain ownership checks, idempotency, DNS record types, TTLs, rate limits, and propagation guarantees are outside inspected scope. A server result is not direct evidence of a DNS resolver's result.

## Public-IP And Connectivity Services

NuciWeb.HTTP 1.7.3 supplies a two-minute process cache, concurrent connectivity checks, and shuffled fallback IP sources. Its source list includes services such as ipify, ident.me, Mullvad, and AWS checkip; it is not one fixed external endpoint. A cache miss may contact connectivity hosts as well as IP lookup hosts. Only trimmed parseable IPv4 values pass the discovery algorithm's address-family check; no additional routability guarantee should be inferred from that check alone.

No provider list, selection override, family switch, or egress allowlist is exposed by this application. Merely configuring a local DNS API does not disable external discovery traffic. Package exceptions escape the service because discovery precedes its catch.

## Logging And Delivery Integrations

NuciLog is an in-process library, not a hosted telemetry service: console and local file destinations are verified. GitHub Actions, NuGet.org, and the remote release helper participate only in build/release, not normal update execution. See [Logging](./logging.md), [Build And Deployment](./build-and-deployment.md), and [Dependencies](./dependencies.md) for exact sources and versions.