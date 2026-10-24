# Repository Documentation

Dynamic DNS Updater Client is a .NET 10 command-line application that discovers an IPv4 address and sends one authenticated DNS-update request to an operator-configured API. This repository owns argument selection, composition, request construction, and operation logging; the remote server owns DNS-provider credentials, validation, mutation, and propagation. It is not a DNS server, scheduler, or API host.

## Start Here

Read [Quick Start](./quick-start.md), [Repository Overview](./repository-overview.md), and [Configuration](./configuration.md). For diagnosis, follow [Startup](./flows/startup.md), [DNS Update](./flows/dns-update.md), and [Troubleshooting](./troubleshooting.md). Component owners should start with [Components](./components/host-and-composition.md), [Invariants](./invariants.md), and [Change Guide](./change-guide.md).

## Root Document Map

| Document | Authority |
|---|---|
| [README](../README.md) | Public project entry point |
| [Security Policy](../SECURITY.md) | Supported versions and vulnerability disclosure |
| [Privacy Policy](../PRIVACY.md) | Published data-handling policy; implementation discrepancies are recorded in [Open Questions](./ambiguities-and-open-questions.md) |
| [License](../LICENSE) | Distribution terms |
| [Contributing](../CONTRIBUTING.md) | Contribution process |
| [Code Of Conduct](../CODE_OF_CONDUCT.md) | Community conduct |

Root architecture and roadmap documents were absent at review. No replacement policy, licence, or roadmap is generated under this directory.

## Documentation Catalogue

### Orientation And Operation

| Document | Contents |
|---|---|
| [Repository Overview](./repository-overview.md) | Purpose, scope, ownership, and entry points |
| [Repository Structure](./repository-structure.md) | Source and delivery navigation |
| [Architecture](./architecture.md) | Runtime topology and dependency direction |
| [Quick Start](./quick-start.md) | Safe setup and first update |
| [Configuration](./configuration.md) | Settings, aliases, precedence, and reload limitations |
| [Build And Deployment](./build-and-deployment.md) | Local build, CI, packaging, and releases |
| [Troubleshooting](./troubleshooting.md) | Symptom-to-cause diagnostic map |
| [FAQ](./faq.md) | Operational questions |

### Contracts And Cross-Cutting Behaviour

| Document | Contents |
|---|---|
| [API Reference](./api-reference/INDEX.md) | No inbound controllers; outbound endpoint inventory |
| [API Usage Examples](./api-usage-examples.md) | Illustrative wire requests and responses |
| [Data Model](./data-model.md) | Settings, request DTO, and response contract |
| [State And Persistence](./state-and-persistence.md) | Memory, package caches, files, and remote state |
| [Concurrency And Scheduling](./concurrency-and-scheduling.md) | Await boundaries, blocking discovery, external scheduling |
| [Dependencies](./dependencies.md) | Versioned packages and upstream evidence |
| [Integrations](./integrations.md) | HTTP construction, discovery, and server boundary |
| [Error Handling](./error-handling.md) | Catch boundaries and observable outcomes |
| [Logging](./logging.md) | Fields, sinks, filtering, redaction, and retention |
| [Security](./security.md) | Implementation threat boundaries; not a disclosure policy |
| [Invariants](./invariants.md) | Contracts and modification hazards |
| [Testing](./testing.md) | Verified cases and untested boundaries |
| [Design Decisions](./design-decisions.md) | Observed choices and explicitly inferred trade-offs |

### Components, Behaviour, And Flows

| Document | Contents |
|---|---|
| [Host And Composition](./components/host-and-composition.md) | Entry point, DI graph, and lifetimes |
| [Input Settings](./components/input-settings.md) | Parser delegation and deterministic alias selection |
| [DNS Record Service](./components/dns-record-service.md) | Update orchestration and exception asymmetry |
| [Integration Models](./components/integration-models.md) | JSON DTO and operation metadata |
| [CLI Behaviour](./behaviour/command-line.md) | Accepted syntax, blank and duplicate values |
| [Update Behaviour](./behaviour/update-record.md) | One-shot effects and success limitations |
| [Startup Flow](./flows/startup.md) | Arguments through service resolution |
| [DNS Update Flow](./flows/dns-update.md) | Discovery through remote response and logging |

### Maintenance

| Document | Contents |
|---|---|
| [Change Guide](./change-guide.md) | Impact sets for common modifications |
| [Documentation Maintenance](./documentation-maintenance.md) | Evidence rules and update checklist |
| [Ambiguities And Open Questions](./ambiguities-and-open-questions.md) | Contradictions, unverified integrations, and coverage gaps |

## Review Metadata

- Last reviewed: 2026-10-24.
- Owner: repository maintainers; no separate documentation owner is declared.
- Coverage: all tracked application sources, three test fixtures, project/solution manifests, three workflows, release wrapper, and editor execution settings inspected.
- Validation: .NET SDK 10.0.112 build succeeded; 503 unit-test cases passed. No live DNS update, release publication, or discovery probe was executed for this investigation.
- Evidence: implementation and tests take precedence over older prose. Version-pinned upstream source was inspected for package-owned behaviour; see [Dependencies](./dependencies.md).