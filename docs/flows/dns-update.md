# DNS Update Flow

## Preconditions

Main has resolved [the DNS service](../../DynamicDnsUpdater.Client/DnsRecordService.cs). Domain/provider may be empty or malformed because parsing selects strings rather than validating business data. API settings may also be invalid because no options validator is registered.

## Discovery Before The Catch

The service emits Started with domain/provider/base URL, creates bearer/client identity metadata, then calls `NuciWeb.HTTP.NetworkUtils.GetPublicIpAddress` synchronously. In version 1.7.3 this consults a two-minute process-local cache; otherwise it checks connectivity and tries shuffled public-IP sources until a trimmed parseable IPv4 address is returned. IPv6 responses are rejected. Connectivity failure or exhausted sources throws before the service's `try`.

The selected IP is stored in the request and appended to later log metadata. No DNS query or previous-address comparison occurs.

## Request Inside The Catch

The service invokes the API client with PUT and endpoint `DnsRecords/{domain}`. The package joins that endpoint to its captured base URL, serializes the DTO to UTF-8 JSON, and attaches bearer and request-metadata headers. See [Integrations](../integrations.md) for URL and header mechanics.

For non-2xx HTTP responses, the pinned API client deserializes `NuciAPI.Responses.NuciApiErrorResponse`; an empty error body becomes a default error response describing HTTP status. For 2xx it deserializes `NuciAPI.Responses.NuciApiSuccessResponse`. The success type's `IsSuccessful` is always true, while the error type's is always false: a body-level `success` property is not an independent validation step on a 2xx response.

## Terminal Branches

| Condition | Consequence | What Main observes |
|---|---|---|
| Successful response object | Info/Success with IP | Normally completed Task |
| Unsuccessful response object | Append response code/message; throw generic HTTP exception; Error/Failure | Normally completed Task if logger succeeds |
| Transport, URI, serialization, null-response, or other exception inside try | Error/Failure with current metadata | Normally completed Task if logger succeeds |
| Started logging or discovery throws | No service catch/Failure log | Fault escapes Main |
| Success logging throws | Catch attempts Error/Failure after possible server mutation | Normal completion if error logger succeeds |
| Failure logging throws | Exception escapes catch | Fault escapes Main |

## Invariant And Uncertainty

The same selected provider and discovered address populate both request and terminal log context; the selected domain populates path and logs. This is traceable submission context, not an atomic record of server state. A timeout can happen after remote mutation. No compensation, retry, read-back, or propagation verification follows.

Current tests verify argument selection and request serialization only. [Testing](../testing.md) records missing flow tests; [Errors](../error-handling.md) records the exit-status consequence.