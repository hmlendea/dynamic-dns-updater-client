# Dependencies

## Direct Runtime Dependencies

Declared by [the application project](../DynamicDnsUpdater.Client/DynamicDnsUpdater.Client.csproj):

| Package | Version | Role |
|---|---|---|
| Microsoft.Extensions.Configuration | 10.0.13 | Configuration abstraction and builder |
| Microsoft.Extensions.Configuration.Binder | 10.0.13 | Bind settings properties |
| Microsoft.Extensions.Configuration.Json | 10.0.13 | Mandatory JSON file provider |
| Microsoft.Extensions.DependencyInjection | 10.0.13 | Service container and lifetimes |
| Microsoft.Extensions.Options.ConfigurationExtensions | 10.0.13 | Bind sections into options |
| NuciAPI | 3.6.2 | Request/response base classes and result codes |
| NuciAPI.Client | 1.2.4 | URL construction, JSON HTTP requests, auth headers, response dispatch |
| NuciCLI.Arguments | 1.0.1 | Strict double-hyphen token parser |
| NuciLog | 1.2.1 | Concrete console/file logger and settings |
| NuciLog.Core | 3.1.0 | Logger interfaces, operations, fields, exception formatting |
| NuciWeb.HTTP | 1.7.3 | Public-IP discovery, connectivity probing, cache |

The executable targets `net10.0`. No SDK pinning file is checked in; workflows select .NET `10.0.x`. Do not conflate the application's direct discovery dependency with the HTTP implementation of NuciAPI.Client: its inspected 1.2.4 source constructs a System.Net.Http client directly.

## Test Dependencies

[The test project](../DynamicDnsUpdater.Client.UnitTests/DynamicDnsUpdater.Client.UnitTests.csproj) targets .NET 10 and references production code. It declares NUnit 5.0.0, NUnit3TestAdapter 6.3.1, Microsoft.NET.Test.Sdk 18.11.0, and coverlet.collector 10.1.0. The collector is private to tests; the test project is nonpackable and marked excluded from code coverage.

## Version-Pinned Evidence

Package-cache repository metadata was checked after restore. These exact tags were verified and relevant source inspected, without decompilation:

| Package source | Evidence used |
|---|---|
| [NuciAPI.Client v1.2.4](https://github.com/hmlendea/nuciapi.Client/tree/v1.2.4) | Client and authorization source: URL joining, direct HttpClient, JSON envelopes, headers |
| [NuciWeb.HTTP v1.7.3](https://github.com/hmlendea/nuciweb.http/tree/v1.7.3) | Architecture documentation and NetworkUtils source: IPv4-only discovery, fallback, connectivity, cache |
| [NuciCLI.Arguments v1.0.1](https://github.com/hmlendea/nucicli.arguments/tree/v1.0.1) | ArgumentParser source: `--` grammar and duplicate overwrite |
| [NuciLog v1.2.1](https://github.com/hmlendea/nucilog/tree/v1.2.1) | Logger/settings source: console, synchronous file append, default format/filter |
| [NuciLog.Core v3.1.0](https://github.com/hmlendea/nucilog.core/tree/v3.1.0) | Logger, LogInfo, LogMessageBuilder: exception fields and sensitive-key masking |
| [NuciAPI v3.6.2](https://github.com/hmlendea/nuciapi/tree/v3.6.2) | Request inheritance and response classes: ignored HMAC token and fixed success flags |

Matching architecture/index URLs were consulted for the first four repositories; source is the decisive evidence where documentation was missing or insufficient. Documentation availability is not a guarantee that a tag's prose is correct: source inspection confirmed the material behaviours recorded here.

## Upgrade Impact

An argument-package upgrade can change public CLI syntax; a networking upgrade can change contacted services and supported address families; an API-client upgrade can change headers/serialization/failure semantics; logging upgrades can change privacy and persistent output. Recheck [Invariants](./invariants.md), [Testing](./testing.md), and [Security](./security.md), rather than treating these as implementation-neutral package bumps.