# API Usage Examples

## Illustrative Outbound Request

The message below illustrates the request constructed by [the service](../DynamicDnsUpdater.Client/DnsRecordService.cs) and NuciAPI.Client 1.2.4. Host, token, metadata, and address are examples, not captured production traffic. Sending it can change DNS.

```http
PUT /DnsRecords/example.com HTTP/1.1
Host: api.example.invalid
Authorization: Bearer <configured-api-key>
Content-Type: application/json; charset=utf-8
X-Client-ID: DynamicDnsUpdater.Client.example-host
X-Request-ID: <generated-uppercase-guid>
X-Timestamp: <URI-escaped-round-trip-local-timestamp>

{"ip":"192.0.2.42","provider":"cloudflare"}
```

The base URL may contribute a path prefix. Client ID is URI-escaped, request ID is generated per HTTP request, and timestamp is URI-escaped. Domain goes in the path, not the JSON body. The application does not set HMAC or an idempotency header. The address shown is a reserved documentation address; production execution discovers an address rather than accepting this field from CLI.

## Response Shapes, Not Server Promises

These are illustrative package-envelope examples. This repository does not establish which exact status/code/message the external DNS endpoint returns.

```json
{"success":true,"code":"SUCCESS","message":"Request completed"}
```

```json
{"success":false,"code":"ERROR","message":"Request failed"}
```

The pinned client selects success versus error response type from HTTP status. The corresponding response type fixes `IsSuccessful`; the service does not independently enforce a body-level `success:false` on 2xx. A non-2xx empty body is converted to a default error response; malformed JSON can throw. A 204 or other empty successful body is not a verified supported server contract.

## Error And Outcome Interpretation

For a deserialized error response, response code/message are appended to Failure logs. For HTTP transport or deserialization exceptions, those fields may be absent. Both paths can return normally to Main because the service catches exceptions. No HTTP response headers are consumed by application code, including retry hints.

See [API Inventory](./api-reference/INDEX.md), [Data Model](./data-model.md), [Errors](./error-handling.md), and [Integration Mechanics](./integrations.md).