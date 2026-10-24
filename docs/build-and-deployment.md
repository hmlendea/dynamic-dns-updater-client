# Build And Deployment

## Local Build And Run

Use .NET 10 SDK. From root, `dotnet restore`, `dotnet build DynamicDnsUpdater.Client.slnx`, and `dotnet test DynamicDnsUpdater.Client.slnx --no-build` match the [CI sequence](../.github/workflows/dotnet.yml). No global SDK pin or package lockfile is checked in. [The application project](../DynamicDnsUpdater.Client/DynamicDnsUpdater.Client.csproj) targets `net10.0`, outputs an executable, and always copies its JSON settings to build output.

Run from [the application directory](../DynamicDnsUpdater.Client/) with double-hyphen options as described in [Quick Start](./quick-start.md). The default build is not explicitly self-contained or RID-specific. Do not claim portable native binaries or .NET-tool installation: project metadata has no `PackAsTool` configuration.

[Editor tasks](../.vscode/tasks.json) expose build, publish, and watch. [Debugger configuration](../.vscode/launch.json) points at the Debug/net10.0 assembly, supplies empty arguments, and sets the application working directory. A publish task exists, but normal release workflows pack rather than publish a runtime distribution.

## Continuous Integration

[dotnet.yml](../.github/workflows/dotnet.yml) runs on push and pull requests targeting `master`, on Ubuntu latest. It uses checkout v2/setup-dotnet v1, installs .NET `10.0.x`, restores, builds without restore, then tests without build. No deployment, coverage upload, artifact publication, or separate security scan is defined there.

## Published-Release Workflows

Both release workflows trigger independently on a published GitHub release. Neither waits for the other or explicitly runs tests.

| Workflow | Steps | Credentials/permissions |
|---|---|---|
| [GitHub Release](../.github/workflows/github-release.yml) | Checkout full history; .NET 10; strip leading `v` from tag; restore; `dotnet pack -c Release --output ./nupkg -p:Version=...`; SHA256 expected package; upload with clobber; append checksum to notes | `contents: write`, GitHub token |
| [NuGet.org Release](../.github/workflows/nuget-release.yml) | Checkout full history; .NET 10; strip leading `v`; restore and pack; NuGet OIDC login; push expected package with skip-duplicate | `contents: read`, `id-token: write`, `NUGET_USER` secret; temporary NuGet API key |

Both assume a package named `DynamicDnsUpdater.Client.{version}.nupkg`. Tests are nonpackable. The executable project has no explicit package version/ID metadata beyond SDK defaults and workflow overrides. These files express publication intent, not evidence that release authentication, packaging contents, or installation succeeded. No signed-binary, SBOM, provenance, runtime archive, or rollback workflow is configured here.

The checksum is calculated for the GitHub-produced package; the independently packed NuGet artefact is not explicitly compared with it. Re-running the GitHub workflow can append another checksum paragraph because notes are appended, not replaced by a stable marker.

## Remote Release Wrapper

[release.sh](../release.sh) sets .NET version `10.0`, downloads `https://raw.githubusercontent.com/hmlendea/dotnet-release-scripts/refs/heads/master/dotnet/10.0.sh` through wget, and pipes it into bash with forwarded arguments. It does not pin a commit or verify a digest, and it does not enable pipefail. Inspect the exact upstream helper before execution; it was not executed or treated as a verified part of the local release algorithm. The historical README URL differs from the wrapper's actual URL.

## Deployment Responsibilities

Operators provision a trusted HTTPS base URL and API key, ensure configuration/log paths are accessible, install a compatible runtime for framework-dependent output, and arrange any repetition externally. Protect configuration copies and log files. There are no checked-in environment profiles, container files, service units, migrations, readiness endpoints, or backup procedures. DNS rollback is a server/provider operation; the client has no rollback command.

See [Configuration](./configuration.md), [Security](./security.md), and [Open Questions](./ambiguities-and-open-questions.md).