# Logging

## Framework And Configuration

The application injects `NuciLog.Core.ILogger`, implemented by NuciLog 1.2.1 with NuciLog.Core 3.1.0. [Program](../DynamicDnsUpdater.Client/Program.cs) binds `NuciLoggerSettings` from [JSON configuration](../DynamicDnsUpdater.Client/appsettings.json) and exposes its options value as a singleton. There is no Microsoft ILogger pipeline or configured external telemetry sink.

## Levels And Lifecycle

[The service](../DynamicDnsUpdater.Client/DnsRecordService.cs) uses only Info and Error: Info/Started before discovery, Info/Success after a successful response, Error/Failure for exceptions inside the request block. Debug, Warning, and Fatal are not called by repository application code. Default minimum level is Info. Package filtering happens before message construction; increasing severity filtering can hide Started/Success.

## Structured Fields

| Field | Presence and meaning |
|---|---|
| Operation | `UpdateDnsRecord` for all service events |
| OperationStatus | Started, Success, or Failure; package formats status uppercase |
| DomainName | Selected domain string |
| Provider | Selected provider string |
| ApiBaseUrl | Value from API options, not independently read from the client's captured URL |
| IpAddress | Added only after discovery; absent from Started |
| ResponseCode, ResponseMessage | Added only when a returned response is unsuccessful |
| Exception, ExceptionMessage, StackTrace | Added by package error formatting when supplied an exception |

[Operation](../DynamicDnsUpdater.Client/Logging/MyOperation.cs) and [key definitions](../DynamicDnsUpdater.Client/Logging/MyLogInfoKey.cs) are the schema authority. The package formats timestamp, source context, uppercase level, and message as four delimited fields. Default timestamp format is `yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK`, using local time. No application-assigned source context, user ID, duration, or success counter exists.

## Correlation

The API client creates an HTTP `X-Request-ID`, client ID, and timestamp, but the service never receives/records that request ID. There are no activity trace IDs, span IDs, or unique invocation IDs in application logs. Domain/provider/time/operation give approximate correlation only, especially under overlapping invocations.

## Sinks, Retention, And Performance

The pinned logger writes via `NuciCLI.NuciConsole.WriteLine` and synchronously appends with `System.IO.File.AppendAllText` when enabled. Shipped settings enable `logfile.log`, relative to working directory. Disabling file output does not disable console output. No database, remote collector, queue, async buffer, file locking, rotation, archival, retention limit, sampling, or log-size limit is configured here. Operators own access control, deletion, rotation, and captured-console retention.

For one invocation there are normally two service lifecycle events. Slow or inaccessible storage delays/fails the update path: Started logging is outside the catch, while Success logging is inside. There is no flush/dispose stage in application code.

## Redaction And Error Logging

The service never intentionally includes the API key in its custom fields. However domain, IP, provider, base URL, response messages, and exception text are exposed. Custom keys do not explicitly mark values sensitive; NuciLog.Core masks fields only when their key is marked sensitive. Sanitizing delimiters/newlines is not privacy redaction. Avoid embedding credentials in the base URL and do not assume arbitrary server messages are safe.

NuciLog.Core's builder includes exception type, message, and sanitized stack trace; it does not separately traverse inner exceptions. No security-specific audit sink exists. Update lifecycle events may assist audit work but are neither complete authorization records nor proof of DNS mutation.

## Runtime Changes And Evidence

Change logger settings before launching the next invocation. JSON watching alone does not make the extracted `IOptions` value live-reloading. Exact defaults and formatting are source-verified at [NuciLog v1.2.1](https://github.com/hmlendea/nucilog/tree/v1.2.1) and [NuciLog.Core v3.1.0](https://github.com/hmlendea/nucilog.core/tree/v3.1.0). No repository tests verify sinks or logging failures. See [Errors](./error-handling.md), [Security](./security.md), and [State](./state-and-persistence.md).