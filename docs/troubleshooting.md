# Troubleshooting

## Symptom Map

| Symptom | Evidence-based cause | Diagnostic/action |
|---|---|---|
| `Invalid argument format: -d` | Parser requires double hyphens | Use `--domain-name` and `--provider-name`; see [CLI](./behaviour/command-line.md) |
| Unknown help/version/equals-style argument | No such options registered | Use separate recognized option/value tokens |
| Missing-parameter exception | Recognized option has no next token | Supply a separate value; beware options consumed as literal values |
| Unexpected alias value wins | Fixed alias priority or last duplicate overwrite | Compare all aliases using [selection rules](./components/input-settings.md) |
| Mandatory JSON startup failure | Settings file absent, inaccessible, or malformed | Launch from application/output directory and verify [settings](../DynamicDnsUpdater.Client/appsettings.json) are copied/accessibly configured |
| Invalid URI/authentication failure | Literal placeholders, missing settings, wrong URL/token | Replace placeholders securely and verify HTTPS/base path with the API operator |
| Started log with no Failure or Success | Discovery or pre-try initialization failed | Inspect unhandled stderr/exception and outbound connectivity; see [Flow](./flows/dns-update.md) |
| Failure log but successful process exit | Service catch swallowed request error | Inspect operation status; verify API/provider state rather than exit code alone |
| Missing response code/message | Transport/serialization failure before error response branch | Inspect exception fields; do not assume a server business-code response existed |
| Success log but stale DNS resolution | No local propagation check; server/provider/resolver state external | Verify API/provider record and resolver caching with the server operator |
| No file output | File sink disabled, path blank, minimum-level filtering, or file I/O failure | Check [logger settings](./configuration.md), working directory, permissions, and console exception |
| Failure after apparent server success | Success logging failed or response was lost | Inspect file permissions and remote record before retrying |
| Output config edits disappear | Source JSON copied on build | Provision settings deliberately and protect copied credentials |
| File edits do not affect running objects | `IOptions` value and captured client URL are not live monitors | Start a new invocation after edits |
| Public-IP discovery fails behind restrictive egress | Package probes connectivity and multiple lookup services | Review exact [dependency egress](./integrations.md); API reachability alone is insufficient |

## Safe Investigation Order

1. Reproduce CLI parsing with existing unit tests, not a production invocation.
2. Check configuration location and placeholders without printing secrets.
3. Determine whether failure occurred before or inside the service catch.
4. Inspect redacted lifecycle/exception fields and remote state if a request may have been sent.
5. Retry only with awareness that the earlier request might already have mutated DNS.

No built-in dry-run, health endpoint, propagation check, or automatic recovery exists. Follow the root [Security Policy](../SECURITY.md) for vulnerabilities rather than posting sensitive diagnostics publicly. See [Testing](./testing.md) for deterministic checks.