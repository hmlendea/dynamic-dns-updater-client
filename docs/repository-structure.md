# Repository Structure

| Location | Responsibility |
|---|---|
| [Solution](../DynamicDnsUpdater.Client.slnx) | Includes executable and unit tests |
| [Application project](../DynamicDnsUpdater.Client/DynamicDnsUpdater.Client.csproj) | .NET 10 executable, runtime packages, configuration copying |
| [Program](../DynamicDnsUpdater.Client/Program.cs) | CLI entry and composition root |
| [DNS service](../DynamicDnsUpdater.Client/DnsRecordService.cs) and [interface](../DynamicDnsUpdater.Client/IDnsRecordService.cs) | Update orchestration contract and implementation |
| [Configuration](../DynamicDnsUpdater.Client/Configuration/) | CLI values and API settings |
| [Requests](../DynamicDnsUpdater.Client/Requests/) | Outbound JSON DTO |
| [Logging](../DynamicDnsUpdater.Client/Logging/) | Operation and structured-field identifiers |
| [Settings file](../DynamicDnsUpdater.Client/appsettings.json) | API placeholders and file-log settings |
| [Unit tests](../DynamicDnsUpdater.Client.UnitTests/) | Argument and serialization fixtures |
| [Workflows](../.github/workflows/) | CI, GitHub package attachment, NuGet publication |
| [Release wrapper](../release.sh) | Downloads and executes a remote release helper |
| [Editor tasks](../.vscode/tasks.json) and [launch settings](../.vscode/launch.json) | Build/publish/watch commands and debugger working directory |
| [Documentation](./INDEX.md) | Evidence-based implementation knowledge |
| [Ignore rules](../.gitignore) | Exclude build output, logs, and other local artefacts |

Build-generated output and NuGet restore metadata are not design authorities. There are no controller, migration, hosted-worker, or provider-adapter source directories in the tracked application.