using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using NUnit.Framework;

using DynamicDnsUpdater.Client.Requests;

namespace DynamicDnsUpdater.Client.UnitTests.Requests
{
    [TestFixture]
    public sealed class PutDnsRecordRequestTests
    {
        private static readonly JsonSerializerOptions WebOptions = new(JsonSerializerDefaults.Web);

        private static readonly JsonSerializerOptions SnakeCaseOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        private static readonly JsonSerializerOptions IgnoreNullOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        [Test, Combinatorial]
        public void GivenRequestValues_WhenSerialising_ThenOnlyTheWireContractIsWritten(
            [Values(
                null, "", "192.0.2.42", "198.51.100.8", "203.0.113.16", "0.0.0.0",
                "255.255.255.255", "127.0.0.1", "::", "::1", "2001:db8::42",
                "::ffff:192.0.2.42", "not-an-address", " 192.0.2.42 ")] string? ipAddress,
            [Values(null, "", "cloudflare", "route53", "Cloudflare", "provider-name", "Yes Man")]
            string? providerName)
        {
            PutDnsRecordRequest request = new()
            {
                IpAddress = ipAddress!,
                DnsProvider = providerName!,
                HmacToken = "dummy-token-not-for-transmission"
            };

            using JsonDocument document = JsonDocument.Parse(JsonSerializer.Serialize(request));
            JsonElement root = document.RootElement;

            Assert.That(root.GetProperty("ip").GetString(), Is.EqualTo(ipAddress));
            Assert.That(root.GetProperty("provider").GetString(), Is.EqualTo(providerName));
            Assert.That(root.EnumerateObject().Count(), Is.EqualTo(2));
        }

        [TestCase("192.0.2.42", "cloudflare")]
        [TestCase("2001:db8::42", "route53")]
        [TestCase("", "")]
        [TestCase(null, null)]
        [TestCase("address\"quoted", "provider\"quoted")]
        [TestCase("address\\escaped", "provider\\escaped")]
        [TestCase("address\nline", "provider\r\nline")]
        [TestCase("address\ttab", "provider\ttab")]
        [TestCase("<address>&", "<provider>&")]
        [TestCase("address\u0000", "provider\u0000")]
        [TestCase("\u00fc.example", "\u0219-provider")]
        [TestCase("\ud83c\udf0d", "\ud83c\udf10")]
        public void GivenRequestValues_WhenRoundTripping_ThenBothValuesArePreserved(
            string? ipAddress,
            string? providerName)
        {
            PutDnsRecordRequest request = new() { IpAddress = ipAddress!, DnsProvider = providerName! };
            string json = JsonSerializer.Serialize(request);
            PutDnsRecordRequest? restored = JsonSerializer.Deserialize<PutDnsRecordRequest>(json);

            Assert.That(restored, Is.Not.Null);
            Assert.That(restored!.IpAddress, Is.EqualTo(ipAddress));
            Assert.That(restored.DnsProvider, Is.EqualTo(providerName));
        }

        [TestCase("{\"ip\":\"192.0.2.42\",\"provider\":\"cloudflare\"}", "192.0.2.42", "cloudflare")]
        [TestCase("{\"provider\":\"route53\",\"ip\":\"2001:db8::42\"}", "2001:db8::42", "route53")]
        [TestCase("{}", null, null)]
        [TestCase("{\"ip\":null,\"provider\":null}", null, null)]
        [TestCase("{\"ip\":\"\",\"provider\":\"\"}", "", "")]
        [TestCase("{\"ip\":\"192.0.2.42\"}", "192.0.2.42", null)]
        [TestCase("{\"provider\":\"cloudflare\"}", null, "cloudflare")]
        [TestCase("{\"ip\":\"192.0.2.42\",\"unknown\":42}", "192.0.2.42", null)]
        [TestCase("{\"IpAddress\":\"192.0.2.42\",\"DnsProvider\":\"cloudflare\"}", null, null)]
        [TestCase("{\"IP\":\"192.0.2.42\",\"PROVIDER\":\"cloudflare\"}", null, null)]
        public void GivenJson_WhenDeserialising_ThenOnlyExactWireNamesAreBound(
            string json,
            string? expectedIpAddress,
            string? expectedProviderName)
        {
            PutDnsRecordRequest? request = JsonSerializer.Deserialize<PutDnsRecordRequest>(json);

            Assert.That(request, Is.Not.Null);
            Assert.That(request!.IpAddress, Is.EqualTo(expectedIpAddress));
            Assert.That(request.DnsProvider, Is.EqualTo(expectedProviderName));
        }

        [TestCase("{\"ip\":42}")]
        [TestCase("{\"ip\":true}")]
        [TestCase("{\"ip\":[]}")]
        [TestCase("{\"ip\":{}}")]
        [TestCase("{\"provider\":42}")]
        [TestCase("{\"provider\":false}")]
        [TestCase("{\"provider\":[]}")]
        [TestCase("{\"provider\":{}}")]
        [TestCase("{")]
        [TestCase("[]")]
        [TestCase("\"request\"")]
        [TestCase("{\"ip\":\"192.0.2.42\",}")]
        public void GivenInvalidJson_WhenDeserialising_ThenAJsonExceptionIsThrown(string json)
            => Assert.That(
                () => JsonSerializer.Deserialize<PutDnsRecordRequest>(json),
                Throws.TypeOf<JsonException>());

        [TestCase("192.0.2.42", "cloudflare")]
        [TestCase("2001:db8::42", "route53")]
        public void GivenNamingPolicies_WhenSerialising_ThenExplicitWireNamesTakePrecedence(
            string ipAddress,
            string providerName)
        {
            PutDnsRecordRequest request = new() { IpAddress = ipAddress, DnsProvider = providerName };
            using JsonDocument webDocument = JsonDocument.Parse(JsonSerializer.Serialize(request, WebOptions));
            using JsonDocument snakeDocument = JsonDocument.Parse(
                JsonSerializer.Serialize(request, SnakeCaseOptions));

            Assert.That(webDocument.RootElement.GetProperty("ip").GetString(), Is.EqualTo(ipAddress));
            Assert.That(webDocument.RootElement.GetProperty("provider").GetString(), Is.EqualTo(providerName));
            Assert.That(snakeDocument.RootElement.GetProperty("ip").GetString(), Is.EqualTo(ipAddress));
            Assert.That(snakeDocument.RootElement.GetProperty("provider").GetString(), Is.EqualTo(providerName));
        }

        [TestCase(null, null, 0)]
        [TestCase("192.0.2.42", null, 1)]
        [TestCase(null, "cloudflare", 1)]
        [TestCase("", "", 2)]
        [TestCase("192.0.2.42", "cloudflare", 2)]
        public void GivenNullSuppression_WhenSerialising_ThenOnlyNullValuesAreOmitted(
            string? ipAddress,
            string? providerName,
            int expectedPropertyCount)
        {
            PutDnsRecordRequest request = new() { IpAddress = ipAddress!, DnsProvider = providerName! };
            using JsonDocument document = JsonDocument.Parse(JsonSerializer.Serialize(request, IgnoreNullOptions));

            Assert.That(document.RootElement.EnumerateObject().Count(), Is.EqualTo(expectedPropertyCount));
            Assert.That(document.RootElement.TryGetProperty("ip", out _), Is.EqualTo(ipAddress is not null));
            Assert.That(document.RootElement.TryGetProperty("provider", out _), Is.EqualTo(providerName is not null));
        }
    }
}