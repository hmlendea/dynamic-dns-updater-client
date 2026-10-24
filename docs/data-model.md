# Data Model

## Application-Owned Types

| Type | Fields | Rules and consumers |
|---|---|---|
| `DynamicDnsUpdater.Client.Configuration.InputSettings` | Read-only `DomainName`, `ProviderName` strings | Parsed once; empty defaults; passed from Main to update service |
| `DynamicDnsUpdater.Client.Configuration.ApiSettings` | Mutable `BaseUrl`, `ApiKey` strings | JSON-bound options; no validation/default initializers; used for client construction, auth, and logs |
| `DynamicDnsUpdater.Client.Requests.PutDnsRecordRequest` | Mutable `IpAddress`, `DnsProvider` strings | Explicit JSON names `ip`, `provider`; no local validators |
| `DynamicDnsUpdater.Client.Logging.MyLogInfoKey` | Named key accessors | Structured log identifiers, not domain entities |
| `DynamicDnsUpdater.Client.Logging.MyOperation` | `UpdateDnsRecord` accessor | Operation identifier, not an update history record |

Source: [input settings](../DynamicDnsUpdater.Client/Configuration/InputSettings.cs), [API settings](../DynamicDnsUpdater.Client/Configuration/ApiSettings.cs), [request](../DynamicDnsUpdater.Client/Requests/PutDnsRecordRequest.cs), and [logging identifiers](../DynamicDnsUpdater.Client/Logging/).

## External Types And Transformations

`NuciAPI.Client.NuciApiRequestAuthorisationInfo` carries bearer token/client ID separately from JSON. The inherited request `HmacToken` is excluded from JSON. Success and error responses derive from `NuciAPI.Responses.NuciApiResponse`, exposing JSON `success`, `code`, and `message`. The concrete success/error types fix the boolean result; HTTP status determines which type the client reads.

The domain string becomes endpoint suffix and log metadata. The provider string becomes request JSON and log metadata. Discovery responses are trimmed, parsed as IPv4, normalized, then placed in request/logs. API key is passed to header construction, never intentionally added as a custom log field. Machine name becomes part of client identity.

## Persistence And Constraints

There are no relational entities, local foreign keys, schema migrations, entity tracking, or domain aggregates. Mutable DNS records exist remotely, not as a local model. [Request tests](../DynamicDnsUpdater.Client.UnitTests/Requests/PutDnsRecordRequestTests.cs) prove serialization acceptance even for null, blank, malformed-looking, and IPv6 strings; that DTO permissiveness does not mean discovery emits IPv6 or the server accepts arbitrary values.

See [State](./state-and-persistence.md) and [Integration Models](./components/integration-models.md) before changing DTO inheritance or wire names.