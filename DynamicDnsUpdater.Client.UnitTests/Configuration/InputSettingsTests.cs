using NUnit.Framework;

using DynamicDnsUpdater.Client.Configuration;

namespace DynamicDnsUpdater.Client.UnitTests.Configuration
{
    [TestFixture]
    public sealed class InputSettingsTests
    {
        [Test]
        public void GivenNoArguments_WhenParsing_ThenBothSettingsAreEmpty()
        {
            InputSettings settings = new([]);

            Assert.That(settings.DomainName, Is.Empty);
            Assert.That(settings.ProviderName, Is.Empty);
        }

        [Test, Combinatorial]
        public void GivenBothOptions_WhenParsing_ThenBothSettingsArePopulated(
            [Values("--d", "--dn", "--domain", "--domain-name")] string domainOption,
            [Values("--p", "--pn", "--provider", "--provider-name")] string providerOption,
            [Values(false, true)] bool isProviderFirst)
        {
            string[] arguments = [domainOption, "test.nucilandia.ro", providerOption, "cloudflare"];

            if (isProviderFirst)
            {
                arguments = [providerOption, "cloudflare", domainOption, "test.nucilandia.ro"];
            }

            InputSettings settings = new(arguments);

            Assert.That(settings.DomainName, Is.EqualTo("test.nucilandia.ro"));
            Assert.That(settings.ProviderName, Is.EqualTo("cloudflare"));
        }

        [Test, Combinatorial]
        public void GivenADomainValue_WhenParsing_ThenTheValueIsPreservedWithoutValidation(
            [Values("--d", "--dn", "--domain", "--domain-name")] string option,
            [Values(
                "test.nucilandia.ro", "nucilandia.ro", "sub.test.nucilandia.ro",
                "TEST.NUCILANDIA.RO", "localhost", "test.nucilandia.ro.",
                "*.nucilandia.ro", "xn--bcher-kva.example", "192.0.2.42", "2001:db8::42",
                "a", "a-b.example", "_service._tcp.example", "not a domain",
                " test.nucilandia.ro ", "--provider", "name=value", "https://example.com/path")]
            string domainName)
        {
            InputSettings settings = new([option, domainName]);

            Assert.That(settings.DomainName, Is.EqualTo(domainName));
            Assert.That(settings.ProviderName, Is.Empty);
        }

        [Test, Combinatorial]
        public void GivenAProviderValue_WhenParsing_ThenTheValueIsPreservedWithoutValidation(
            [Values("--p", "--pn", "--provider", "--provider-name")] string option,
            [Values(
                "cloudflare", "Cloudflare", "CLOUDFLARE", "route53", "digital-ocean",
                "provider_name", "provider.name", "42", "a", "Yes Man", "Grumpy Cat",
                " cloudflare ", "--domain", "provider=value", "provider/path", "provider:port")]
            string providerName)
        {
            InputSettings settings = new([option, providerName]);

            Assert.That(settings.DomainName, Is.Empty);
            Assert.That(settings.ProviderName, Is.EqualTo(providerName));
        }

        [Test, Combinatorial]
        public void GivenBlankValues_WhenParsing_ThenBothSettingsAreEmpty(
            [Values("--d", "--dn", "--domain", "--domain-name")] string domainOption,
            [Values("--p", "--pn", "--provider", "--provider-name")] string providerOption,
            [Values("", " ", "\t", "\r\n")] string blankValue)
        {
            InputSettings settings = new([domainOption, blankValue, providerOption, blankValue]);

            Assert.That(settings.DomainName, Is.Empty);
            Assert.That(settings.ProviderName, Is.Empty);
        }

        [TestCase("--d", "--dn")]
        [TestCase("--d", "--domain")]
        [TestCase("--d", "--domain-name")]
        [TestCase("--dn", "--domain")]
        [TestCase("--dn", "--domain-name")]
        [TestCase("--domain", "--domain-name")]
        public void GivenCompetingDomainAliases_WhenParsingInEitherOrder_ThenAliasPriorityWins(
            string preferredOption,
            string otherOption)
        {
            InputSettings preferredFirst = new(
                [preferredOption, "preferred.example", otherOption, "other.example"]);
            InputSettings preferredLast = new(
                [otherOption, "other.example", preferredOption, "preferred.example"]);

            Assert.That(preferredFirst.DomainName, Is.EqualTo("preferred.example"));
            Assert.That(preferredLast.DomainName, Is.EqualTo("preferred.example"));
            Assert.That(preferredFirst.ProviderName, Is.Empty);
            Assert.That(preferredLast.ProviderName, Is.Empty);
        }

        [TestCase("--p", "--pn")]
        [TestCase("--p", "--provider")]
        [TestCase("--p", "--provider-name")]
        [TestCase("--pn", "--provider")]
        [TestCase("--pn", "--provider-name")]
        [TestCase("--provider", "--provider-name")]
        public void GivenCompetingProviderAliases_WhenParsingInEitherOrder_ThenAliasPriorityWins(
            string preferredOption,
            string otherOption)
        {
            InputSettings preferredFirst = new([preferredOption, "cloudflare", otherOption, "route53"]);
            InputSettings preferredLast = new([otherOption, "route53", preferredOption, "cloudflare"]);

            Assert.That(preferredFirst.ProviderName, Is.EqualTo("cloudflare"));
            Assert.That(preferredLast.ProviderName, Is.EqualTo("cloudflare"));
            Assert.That(preferredFirst.DomainName, Is.Empty);
            Assert.That(preferredLast.DomainName, Is.Empty);
        }

        [Test, Combinatorial]
        public void GivenBlankPreferredAliases_WhenParsing_ThenTheNextNonblankAliasesAreUsed(
            [Values("", " ", "\t", "\r\n")] string blankValue,
            [Values("--dn", "--domain", "--domain-name")] string domainFallback,
            [Values("--pn", "--provider", "--provider-name")] string providerFallback)
        {
            InputSettings settings = new(
                ["--d", blankValue, domainFallback, "test.nucilandia.ro",
                 "--p", blankValue, providerFallback, "cloudflare"]);

            Assert.That(settings.DomainName, Is.EqualTo("test.nucilandia.ro"));
            Assert.That(settings.ProviderName, Is.EqualTo("cloudflare"));
        }

        [TestCase("", "", "")]
        [TestCase(" ", "\t", "\r\n")]
        [TestCase("first.example", "second.example", "third.example")]
        public void GivenAllDomainAliases_WhenParsing_ThenTheFirstNonblankAliasWins(
            string firstValue,
            string secondValue,
            string thirdValue)
        {
            InputSettings settings = new(
                ["--domain-name", "fallback.example", "--domain", thirdValue,
                 "--dn", secondValue, "--d", firstValue]);
            string expected = "fallback.example";

            if (!string.IsNullOrWhiteSpace(firstValue))
            {
                expected = firstValue;
            }

            Assert.That(settings.DomainName, Is.EqualTo(expected));
            Assert.That(settings.ProviderName, Is.Empty);
        }

        [TestCase("--d")]
        [TestCase("--dn")]
        [TestCase("--domain")]
        [TestCase("--domain-name")]
        public void GivenARepeatedDomainOption_WhenParsing_ThenTheLastValueWins(string option)
        {
            InputSettings settings = new([option, "first.example", option, "last.example"]);

            Assert.That(settings.DomainName, Is.EqualTo("last.example"));
            Assert.That(settings.ProviderName, Is.Empty);
        }

        [TestCase("--p")]
        [TestCase("--pn")]
        [TestCase("--provider")]
        [TestCase("--provider-name")]
        public void GivenARepeatedProviderOption_WhenParsing_ThenTheLastValueWins(string option)
        {
            InputSettings settings = new([option, "cloudflare", option, "route53"]);

            Assert.That(settings.ProviderName, Is.EqualTo("route53"));
            Assert.That(settings.DomainName, Is.Empty);
        }

        [TestCase("", "")]
        [TestCase(" ", "\t")]
        [TestCase("\r\n", " ")]
        public void GivenDuplicatesEndingWithBlanks_WhenParsing_ThenOtherAliasesProvideTheValues(
            string domainBlank,
            string providerBlank)
        {
            InputSettings settings = new(
                ["--d", "discarded.example", "--d", domainBlank, "--domain", "fallback.example",
                 "--p", "discarded", "--p", providerBlank, "--provider", "cloudflare"]);

            Assert.That(settings.DomainName, Is.EqualTo("fallback.example"));
            Assert.That(settings.ProviderName, Is.EqualTo("cloudflare"));
        }

        [TestCase("test.nucilandia.ro", "cloudflare")]
        [TestCase("other.example", "route53")]
        public void GivenSeparateInstances_WhenParsing_ThenSettingsDoNotLeakBetweenInstances(
            string domainName,
            string providerName)
        {
            InputSettings populated = new(["--domain", domainName, "--provider", providerName]);
            InputSettings empty = new([]);

            Assert.That(populated.DomainName, Is.EqualTo(domainName));
            Assert.That(populated.ProviderName, Is.EqualTo(providerName));
            Assert.That(empty.DomainName, Is.Empty);
            Assert.That(empty.ProviderName, Is.Empty);
        }

        [TestCase(63)]
        [TestCase(64)]
        [TestCase(253)]
        [TestCase(254)]
        [TestCase(4096)]
        public void GivenLongValues_WhenParsing_ThenNoImplicitLengthLimitIsApplied(int length)
        {
            string value = new('a', length);
            InputSettings settings = new(["--domain", value, "--provider", value]);

            Assert.That(settings.DomainName, Is.EqualTo(value));
            Assert.That(settings.ProviderName, Is.EqualTo(value));
        }
    }
}