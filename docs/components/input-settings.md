# Input Settings

## Ownership And Contract

[Input settings](../../DynamicDnsUpdater.Client/Configuration/InputSettings.cs) implements `DynamicDnsUpdater.Client.Configuration.InputSettings`. It converts a token array into two immutable string properties. It delegates token grammar and duplicate storage to NuciCLI.Arguments 1.0.1, then applies its own alias precedence and blank filtering. It does not validate DNS names, known providers, maximum lengths, or network safety.

## Exact Algorithm

1. Register eight option names after stripping leading hyphens from internal alias strings. Every registration is optional with empty-string default.
2. Call `NuciCLI.Arguments.ArgumentParser.ParseArgs` once. The package accepts only tokens starting with `--`, checks the exact registered name, dequeues the next token as value without interpreting it, and stores it by name. Duplicate names overwrite.
3. For each setting, walk the alias array in declaration order. Read each parsed value and return the first value that is not null/empty/whitespace.
4. Return empty string when all aliases are absent or blank. Preserve the selected value verbatim.

Consequently the internal `-d`/`-p` array entries normalize to names `d`/`p`, but users must supply `--d`/`--p`. Internal spelling does not override the package parser grammar.

## Causal Edge Cases

| Input | Result |
|---|---|
| No tokens | Both strings empty; Main still invokes update |
| Same alias twice | Last value survives before alias selection |
| Preferred alias blank, lower alias populated | Lower alias supplies value |
| Preferred alias populated but appears first | Preferred value still wins; token order is irrelevant across aliases |
| `--domain --provider` | Literal `--provider` becomes domain value; provider remains empty |
| `--domain --provider cloudflare` | Remaining positional `cloudflare` is rejected |
| `--domain=name`, `--help`, case-changed names | Unknown argument exception |
| Single-hyphen option or positional token | Invalid-format exception |
| Recognized trailing option without value | Argument-null exception naming the option |

## Evidence And Dependants

[InputSettingsTests](../../DynamicDnsUpdater.Client.UnitTests/Configuration/InputSettingsTests.cs) covers combinations, ordering, fixed alias precedence, blank fallbacks, duplicates, instance independence, and lengths through 4096 characters. [Invalid-argument tests](../../DynamicDnsUpdater.Client.UnitTests/Configuration/InputSettingsInvalidArgumentsTests.cs) covers grammar, unknown names, missing values, null input, and tokens consumed as values.

Main passes values directly into the service: domain becomes part of a URL; provider becomes JSON and logging metadata. A validation or normalization change therefore affects [HTTP integration](../integrations.md), logs, public CLI compatibility, and both test fixtures. See [CLI Behaviour](../behaviour/command-line.md).