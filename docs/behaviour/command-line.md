# CLI Behaviour

## Supported Invocation

Launch from [the application directory](../../DynamicDnsUpdater.Client/) with `dotnet run -- --domain-name example.com --provider-name cloudflare`. One invocation selects one domain/provider pair. Provider spelling and domain validity are not checked locally.

Accepted aliases are `--d`, `--dn`, `--domain`, `--domain-name` and `--p`, `--pn`, `--provider`, `--provider-name`. Option names are case-sensitive. Use separate value tokens; `--domain=example.com` is unsupported. Single-hyphen forms are rejected despite historical README examples.

## Selection Rules

All options are optional and default to empty. Whitespace-only values are ignored during selection, but surrounding whitespace on a nonblank value is preserved. Fixed alias priority wins over ordering across aliases; the last occurrence wins only within the same alias. See [the exact parser algorithm](../components/input-settings.md) and [Configuration](../configuration.md).

There is no help, version, dry-run, batch, or interactive prompt path. Unknown options throw before the logger is created. Empty input is not a usage error: the program proceeds toward discovery and a request with empty domain/provider.

## Verification

[Valid-input tests](../../DynamicDnsUpdater.Client.UnitTests/Configuration/InputSettingsTests.cs) and [invalid-input tests](../../DynamicDnsUpdater.Client.UnitTests/Configuration/InputSettingsInvalidArgumentsTests.cs) demonstrate these semantics without network access. Quoting values containing spaces is the invoking shell's responsibility; the class receives an already-tokenized string array.