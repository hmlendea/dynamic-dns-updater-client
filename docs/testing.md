# Testing

## Organization And Execution

[The test project](../DynamicDnsUpdater.Client.UnitTests/DynamicDnsUpdater.Client.UnitTests.csproj) uses NUnit, NUnit3TestAdapter, and Microsoft.NET.Test.Sdk, references production code, and targets .NET 10. It is nonpackable, enables nullable analysis, and includes coverlet.collector. No integration-test or end-to-end project exists.

From the root, build [the solution](../DynamicDnsUpdater.Client.slnx), then run `dotnet test DynamicDnsUpdater.Client.slnx --no-build`. To obtain an explicit test summary, use `dotnet test DynamicDnsUpdater.Client.UnitTests/DynamicDnsUpdater.Client.UnitTests.csproj --no-build --logger 'console;verbosity=normal'`. Test data is synthetic; these fixtures do not perform DNS updates.

## Verified Coverage

| Fixture | Demonstrated semantics |
|---|---|
| [InputSettingsTests](../DynamicDnsUpdater.Client.UnitTests/Configuration/InputSettingsTests.cs) | Empty defaults; all accepted alias combinations/orderings; fixed alias priority; blank fallback; duplicates; independent instances; unvalidated case/whitespace/long values |
| [InputSettingsInvalidArgumentsTests](../DynamicDnsUpdater.Client.UnitTests/Configuration/InputSettingsInvalidArgumentsTests.cs) | Null arrays; single-hyphen/positional format rejection; unknown/case-changed/help/version/equals-style options; missing values; option tokens consumed as values |
| [PutDnsRecordRequestTests](../DynamicDnsUpdater.Client.UnitTests/Requests/PutDnsRecordRequestTests.cs) | Two exact default JSON properties; ignored inherited HMAC token; escaping and Unicode round trips; exact wire-name matching; invalid JSON; naming-policy precedence; optional null suppression |

Combinatorial attributes expand methods into many cases. The verified run on 2026-10-24 used SDK 10.0.112: build succeeded and 503 cases passed. This is an observed baseline, not a coverage percentage or future guarantee.

## Missing Coverage And Safe Test Design

No fixture exercises `DynamicDnsUpdater.Client.Program`, `DynamicDnsUpdater.Client.DnsRecordService`, the HTTP wire transport, actual public-IP discovery, configuration file resolution/reload, log sinks/redaction, exit behavior, release artefacts, or live server/provider semantics.

For service changes, derive cases for Started/discovery failures outside the catch; successful response; unsuccessful response with code/message; send/serialization exceptions; null response; Success logging failure; Failure logging failure; and uncertain remote state after timeout. Static discovery currently obstructs isolation; make that seam explicit if introducing deterministic tests. Do not silently contact public services from unit tests.

For input changes, preserve both parser-owned syntax and locally owned alias selection. For DTO changes, retain inherited-property exclusion and test the actual package HTTP serializer separately from direct System.Text.Json tests.

## Coverage And CI

The collector supports `dotnet test --collect:'XPlat Code Coverage'`; reports normally appear in a generated test-results directory. No coverage threshold, published dashboard, or uploaded coverage artefact is configured in [CI](../.github/workflows/dotnet.yml), and coverage collection was not run for this documentation change. The test assembly is marked excluded from code coverage; that is not evidence of application coverage.