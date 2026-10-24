# FAQ

## Does It Run Continuously?

No. [Main](../DynamicDnsUpdater.Client/Program.cs) awaits one update and returns. Schedule repetitions externally; no production schedule is provided.

## Why Does `-d` Fail?

The pinned parser accepts double-hyphen names only. Use `--d` or `--domain-name`. Internal alias normalization does not enable short-option grammar. See [CLI Behaviour](./behaviour/command-line.md).

## Must I Supply Both Options?

Not for local parsing: both default to empty. The program still proceeds; server-side acceptance is unknown. Supply authorized valid values for a meaningful update.

## Does The Provider Name Load A Plugin?

No. It is sent as JSON to the external server. Provider support and credentials are server-owned.

## Can I Select IPv6 Or Supply An IP Manually?

No CLI option does that. Pinned NuciWeb.HTTP discovery returns IPv4 only. The DTO's ability to serialize an IPv6 string is not an implemented discovery feature.

## Does A Zero Exit Status Mean DNS Changed?

No. Request failures can be logged and swallowed. Even a Success log does not verify DNS propagation. See [Errors](./error-handling.md).

## Is The API Key Expanded From An Environment Variable?

No. Placeholder strings are literal JSON values, and no environment configuration provider is registered. See [Configuration](./configuration.md).

## Are Logs Persisted?

Yes, with shipped settings: console plus file append to `logfile.log`. Disable the file sink or manage its permissions/rotation explicitly. See [Logging](./logging.md) and the recorded [policy discrepancy](./ambiguities-and-open-questions.md).

## Can I Install It As A Dotnet Tool?

No tool packaging configuration is present. Release workflows pack a NuGet artefact, but that alone does not establish executable-tool installation support. See [Build And Deployment](./build-and-deployment.md).