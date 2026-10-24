# DNS Record Service

## Purpose And Scope

[DnsRecordService](../../DynamicDnsUpdater.Client/DnsRecordService.cs) implements `DynamicDnsUpdater.Client.IDnsRecordService.Update`. It owns discovery-to-request orchestration and operation logging, not provider-specific DNS operations. The interface accepts two strings and returns `System.Threading.Tasks.Task`; no update result or cancellation token is returned/accepted.

## Execution And Information Boundaries

1. Create log fields for domain, provider, and configured API base URL; emit Info/Started.
2. Create authorization information containing configured API key and client ID `DynamicDnsUpdater.Client.{machine-name}`.
3. Synchronously call `NuciWeb.HTTP.NetworkUtils.GetPublicIpAddress` and construct `DynamicDnsUpdater.Client.Requests.PutDnsRecordRequest` with that IP and provider.
4. Append the address field to the enumerable log metadata.
5. Enter `try`, await one `NuciAPI.Client.INuciApiClient.SendRequestAsync` using PUT, success response type `NuciAPI.Responses.NuciApiSuccessResponse`, and relative endpoint `DnsRecords/{domainName}`.
6. If the returned response reports unsuccessful, append response code/message and throw a generic HTTP request exception.
7. Otherwise emit Info/Success. Catch any exception from this block, log Error/Failure with exception and accumulated fields, and return normally if error logging succeeds.

The placement of step 5 matters: IP lookup, Started logging, settings access, and authorization/request initialization are outside the catch. A lookup failure produces no service Failure log. Conversely, a Success-log file-write failure is caught as update failure even if the remote update already happened.

## State And Failure Contract

There is no address comparison, rollback, local record cache, retry, provider enumeration, or DNS read-back. The package discovery cache is separate from DNS state. The server might mutate DNS before a lost response causes a Failure log. Normal `Task` completion after catch is not evidence of mutation success.

No service-specific tests currently exist; the argument/DTO fixtures do not exercise HTTP, discovery, logging, or this exception boundary. To change this area safely, derive tests from [the full flow](../flows/dns-update.md) and [failure taxonomy](../error-handling.md), including log failures and discovery failures.