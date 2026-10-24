# Design Decisions

## Evidence Standard

No ADR or original design rationale is checked in. The choices below are observable implementation facts; trade-offs are analysis, not claims about author intent. See [Architecture](./architecture.md) for topology.

| Observed choice | Consequence and inferred trade-off |
|---|---|
| One-shot console entry | External scheduling stays separate, but ongoing synchronization is not automatic |
| Remote API owns DNS integration | Provider secrets/SDKs stay outside this client; validation and mutation guarantees cannot be inferred locally |
| Small DI service boundary | HTTP client and logger are replaceable dependencies; static discovery remains a testing obstacle |
| Delegate syntax to NuciCLI.Arguments | Reuses parsing, but pinned package grammar overrides intuitive interpretation of internal `-d` names |
| Deterministic alias priority | Equivalent settings are chosen independently of token ordering across aliases; this differs from a global last-option-wins convention |
| Explicit DTO JSON names | Wire names survive naming-policy changes; null/malformed strings still require external validation |
| Package success/error response types | Simplifies local dispatch, but 2xx response-body success is not independently checked |
| Catch request failures and log | Produces diagnostics without propagating most update failures to the process exit status |
| Discovery before try | Discovery errors remain unhandled by service error logging; intent is unknown |
| Default console/file logger | Local diagnostics persist, requiring operator privacy/retention management |
| JSON watching with `IOptions` | Watch registration exists without effective live options monitoring |
| Independent GitHub/NuGet packing | Publication channels are separate, without explicit byte-identity checks or test gates in the release jobs |

Before preserving a surprising behavior as intentional policy, consult [Invariants](./invariants.md), [Tests](./testing.md), and [Open Questions](./ambiguities-and-open-questions.md). Tests establish contracts where present, not historical motivations.