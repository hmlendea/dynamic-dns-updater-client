# Documentation Maintenance

## Ownership And Evidence

Repository maintainers own these documents; there is no separate assigned documentation team. Review date and coverage live in [INDEX](./INDEX.md). Prefer implementation, then tests/configuration/manifests/workflows, then exact-version dependency source. Mark inferred rationale, unverified server contracts, and unknown deployment state explicitly.

Do not turn tests of direct serialization into proof of HTTP transport behavior, or normal Task completion into proof of DNS mutation. Do not infer a hosted service, scheduler, provider adapter, or database from generic architectural patterns.

## Update Checklist

1. Trace changed input through callers, transformations, external effects, failures, and dependants.
2. Reconcile component docs with the affected end-to-end flow and [Invariants](./invariants.md).
3. Recheck package tags on dependency upgrades; never substitute default-branch behavior silently.
4. Record test command/outcome and gaps without claiming coverage percentages not measured.
5. Verify every relative file/directory link and ensure every documentation file appears in the master catalogue.
6. Assess root README/security/privacy/architecture/roadmap material; change only affected documents and preserve required templates. Policies, roadmap, and licence stay at root.
7. Keep headings, terminology, and relative navigation consistent across the knowledge base.

## Boundaries And Open Items

[Open Questions](./ambiguities-and-open-questions.md) records unresolved source/prose mismatches and untested boundaries, not committed roadmap promises. Remove a gap only after evidence resolves it. Keep [the root architecture overview](../ARCHITECTURE.md) synchronized with architectural changes and [the runtime architecture notes](./architecture.md) complementary rather than copying its contents.

This documentation task does not change application behavior or silently correct production defects. Dependency source was inspected online; real discovery/API calls and release scripts were not executed.