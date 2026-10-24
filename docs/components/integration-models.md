# Integration Models

## Request DTO

[PutDnsRecordRequest](../../DynamicDnsUpdater.Client/Requests/PutDnsRecordRequest.cs) defines `DynamicDnsUpdater.Client.Requests.PutDnsRecordRequest`, derived from `NuciAPI.Requests.NuciApiRequest`. Explicit JSON names are `ip` for `IpAddress` and `provider` for `DnsProvider`. Both properties are mutable strings without validation. The inherited `HmacToken` is JSON-ignored by the package base class; the application does not sign requests.

The domain is deliberately absent from JSON because the service places it in the URL. Authorization is deliberately separate from the DTO. No response DTO is defined locally; the service uses package response types.

## Logging Identifiers

[MyOperation](../../DynamicDnsUpdater.Client/Logging/MyOperation.cs) supplies operation name `UpdateDnsRecord`. [MyLogInfoKey](../../DynamicDnsUpdater.Client/Logging/MyLogInfoKey.cs) supplies domain, IP, provider, API-base-URL, response-code, and response-message keys. Accessors construct new identifier objects on each access; these are metadata declarations, not stored operation state.

## Tests And Change Impact

[PutDnsRecordRequestTests](../../DynamicDnsUpdater.Client.UnitTests/Requests/PutDnsRecordRequestTests.cs) verifies exact JSON keys, exactly two default-serialized properties, inherited-token exclusion, naming-policy overrides, null suppression, escaping/Unicode round trips, invalid JSON, and default case-sensitive deserialization. Those direct System.Text.Json tests do not constitute an HTTP integration test of the package serializer.

Changing a JSON name requires server coordination and updates to tests, [Data Model](../data-model.md), and [API Examples](../api-usage-examples.md). Changing log-key names affects operator queries and [Logging](../logging.md), not the wire request schema.