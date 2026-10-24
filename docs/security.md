# Security

## Scope And Policy

This document explains implementation trust boundaries, not vulnerability disclosure or a new security notice. Follow the root [Security Policy](../SECURITY.md) for reporting and support scope, [Privacy Policy](../PRIVACY.md) for the published policy, and [License](../LICENSE) for legal terms. Code/policy discrepancies are explicitly tracked in [Open Questions](./ambiguities-and-open-questions.md).

## Trust Boundaries

| Boundary | Evidence and risk | Practical operator control |
|---|---|---|
| Local configuration | Plaintext API key; file copied to output | Restrict source/output/backups; never commit real credentials |
| API destination | Operator-selected URL; no HTTPS enforcement | Use a trusted HTTPS endpoint; validate base path |
| CLI to URL | Domain interpolated without explicit escaping/validation | Supply trusted domain values; do not expose raw invocation to untrusted input |
| Machine to discovery services | Connectivity probes and public-IP lookup | Review pinned dependency egress before restrictive deployment |
| Host identity to API | Machine name included in client ID | Assess host-identity disclosure |
| API/network text to logs | Response and exception messages retained | Limit log access and retention; sanitize shared diagnostics |
| Release wrapper to shell | Mutable upstream script downloaded and executed | Inspect/pin helper independently before execution |

## Authentication And Authorization

The service supplies `NuciAPI.Client.NuciApiRequestAuthorisationInfo.BearerToken`. The pinned client omits Authorization if token is empty. There is no local credential validation, refresh, role check, or permission model; the remote API must authorize updates. HMAC exists in dependency types but is not configured by this application. HTTPS is contingent on the operator's URL, not guaranteed by code.

## Logs And Sensitive Data

File logging is enabled by default and persists domain/IP/base URL and failures. The application does not implement blanket masking of those fields. API keys are not explicitly added to custom logs, but unsafe URL credentials or server/exception messages could still expose sensitive content. See [Logging](./logging.md) for exact redaction and retention limits.

## Negative Capabilities And Verification Limits

There is no inbound listener, database, remote command endpoint, or browser session. Absence of these surfaces does not establish overall security. No repository integration tests establish credential secrecy, TLS enforcement, domain path safety, server authorization, or secure package publication. Release workflows use OIDC for NuGet publishing and a GitHub token for release writes, but successful publication was not tested during this documentation task.

See [Integrations](./integrations.md), [Dependencies](./dependencies.md), and [Build And Deployment](./build-and-deployment.md) before changing network or supply-chain boundaries.