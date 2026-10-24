# Ambiguities And Open Questions

## Observed Contradictions

| Issue | Evidence | Interpretation and next verification |
|---|---|---|
| Single-hyphen options advertised | [README](../README.md) examples versus [invalid-input tests](../DynamicDnsUpdater.Client.UnitTests/Configuration/InputSettingsInvalidArgumentsTests.cs) and pinned parser | Actual accepted forms start with `--`; decide whether to change prose or parser contract |
| Persistent logging denied by privacy prose | [Privacy Policy](../PRIVACY.md) versus [JSON](../DynamicDnsUpdater.Client/appsettings.json) and NuciLog file append | Shipped defaults persist logs; root privacy/README privacy sections need a coordinated template-compliant policy review |
| Policy says integrations only receive data | [Privacy Policy](../PRIVACY.md) versus discovery and API response processing | Public-IP data and API response data are received; policy wording requires review |
| Privacy prose omits machine identity | Client ID construction in [service](../DynamicDnsUpdater.Client/DnsRecordService.cs) | Machine name crosses the API boundary; include in a future policy review |
| Automatic synchronization implied | [README](../README.md) versus one awaited update in [Main](../DynamicDnsUpdater.Client/Program.cs) | Requires external repeated invocation; no schedule exists here |
| Release-helper URL drift | [README](../README.md), [wrapper](../release.sh), profile README template | Wrapper's actual URL is documented in [Build](./build-and-deployment.md); fixed template prose conflicts with implementation and should not be silently treated as evidence |

## Unverified External Contracts

- The server repository was not inspected: supported providers, record types/TTLs, domain authorization, status catalogue, idempotency, throttling, and propagation remain unknown.
- Release workflow definitions were inspected, not executed; public NuGet availability, artefact contents, trusted-publishing registration, and downstream installability are not established by their presence.
- Network discovery behavior is source-verified at the pinned package version, not live-tested against current services or the deployment's firewall/proxy environment.
- Mandatory JSON loading and package behavior are understood from implementation; no runtime test establishes every deployment's file-provider resolution behavior.

## Missing Tests And Potential Hazards

- Request failures are swallowed; no test establishes a nonzero automation result. Desired exit semantics are a product decision, not documented intent.
- Discovery and Started logging are outside the catch; whether this asymmetry is intentional is unknown.
- Successful HTTP status selects a permanently successful response type; the endpoint's treatment of body-level failure on 2xx is unverified.
- Domain input is interpolated into a URL without explicit escaping; no tests cover path/query/fragment safety.
- Logs may contain identifiers, base URLs, response text, and exception messages; no masking/retention tests exist.
- No integration tests verify package serializer, auth headers, logs, configuration reload, or server mutation. No concurrency tests cover overlapping invocations.

## Root Documentation Validation

The profile's README validator assumes templates reside inside the repository; this checkout stores them in the editor profile. With that path corrected in memory, both the original and updated README produce the same two fixed-prose errors: the validator treats the instruction-file introduction and mandatory-inclusion directive as required README content. Its heading-count/table heuristics also warn on existing content and ordinary populated tables. The added Documentation, FAQ, and Troubleshooting sections use the template's headings/fixed prose; this is not a claim that the entire pre-existing README meets the current template. Root policy and unrelated README regeneration are deferred rather than replacing verified implementation evidence with template text.

These are evidence-backed gaps, not implementation changes or a root roadmap. See [Testing](./testing.md), [Errors](./error-handling.md), and [Security](./security.md).