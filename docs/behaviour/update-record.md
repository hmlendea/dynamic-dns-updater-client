# Update Behaviour

## Observable Outcome

The client discovers a normalized IPv4 address, requests an update for the selected provider/domain, and logs an operation status. It does not continuously keep a record synchronized by itself. An external scheduler must rerun the executable when repeated updates are desired; no scheduler or schedule is supplied.

[The service](../../DynamicDnsUpdater.Client/DnsRecordService.cs) makes one API send call on every invocation that reaches the request block. It never checks whether the address changed or whether the provider already has that address. The server controls record existence, provider support, validation, permissions, mutation, and propagation.

## Success And Failure

Success means the package returned a successful response object and Success logging completed. It is not a DNS propagation check. Request/response exceptions within the catch boundary become Failure logs without rethrow; discovery exceptions are outside it and escape to Main. A logging error may obscure either outcome.

An interrupted or failed request can leave remote state unknown. Do not assume Failure means no mutation occurred, or that repeating PUT is harmless without verifying the server's contract. No retry or idempotency key is implemented locally.

See [DNS Update Flow](../flows/dns-update.md), [Error Handling](../error-handling.md), and [Troubleshooting](../troubleshooting.md). Current unit tests cover the selected input and DTO, not the observable update lifecycle.