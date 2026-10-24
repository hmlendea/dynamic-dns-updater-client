# Error Handling

## Failure Taxonomy

| Failure point | Handler | Observable result and recovery |
|---|---|---|
| Null/invalid CLI tokens | No application catch | Exception escapes Main before logger exists; correct token grammar |
| Missing/malformed mandatory JSON | No application catch | Startup stops; restore valid accessible settings |
| Options/DI/client construction | No application catch | Service is not invoked; inspect configuration and package dependencies |
| Started logger | Outside service try | Exception escapes; update not submitted |
| Discovery/connectivity | Outside service try | Started may exist without Failure; no API submission yet |
| URI/auth/serialization/transport in send | Service catch | Error/Failure logged; Task normally completes |
| Non-2xx parsed API response | Explicit unsuccessful-response branch, then catch | Response code/message logged with generic HTTP exception; Task normally completes |
| Malformed/null response handling | Service catch | Failure log; response metadata may be unavailable |
| Success logging | Service catch | Failure may be logged even after successful API mutation |
| Failure logging | No enclosing application catch | Logger exception escapes and can obscure original error |

Source: [Program](../DynamicDnsUpdater.Client/Program.cs), [service](../DynamicDnsUpdater.Client/DnsRecordService.cs), and [versioned package implementations](./dependencies.md).

## Exit And Result Semantics

Main returns `Task`, not an integer exit code, and the service returns no result. No code explicitly sets `Environment.ExitCode`. When request failures are swallowed and error logging succeeds, Main completes normally, usually producing a successful process exit. Escaping exceptions fault the entry point; exact runtime exit codes are not specified by application code.

Automation must not equate normal completion with a successful update. Read operation status and verify remote state as appropriate. There is no retry, backoff, exception translation at startup, rollback, or dead-letter storage.

## Unknown Remote State

Transport failure does not prove the server did not apply an update. Replaying may be appropriate operationally, but the server's idempotency contract is unverified. Do not document all failures as safe to retry. No post-update DNS query resolves this ambiguity.

## Exception Information

The generic exception for unsuccessful responses says only that the API request failed; response code/message live in separate fields. NuciLog.Core formats exception type, message, and stack trace; the inspected builder does not separately enumerate inner exceptions. API error-body text and exception messages must be treated as potentially sensitive.

No current test exercises the service catch boundary or exit behavior. See [Testing](./testing.md) and [Troubleshooting](./troubleshooting.md).