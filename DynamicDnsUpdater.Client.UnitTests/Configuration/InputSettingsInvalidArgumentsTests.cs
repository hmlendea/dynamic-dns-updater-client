using System;

using NUnit.Framework;

using DynamicDnsUpdater.Client.Configuration;

namespace DynamicDnsUpdater.Client.UnitTests.Configuration
{
    [TestFixture]
    public sealed class InputSettingsInvalidArgumentsTests
    {
        [Test]
        public void GivenNullArguments_WhenParsing_ThenAnArgumentNullExceptionIsThrown()
            => Assert.That(
                () => new InputSettings(null!),
                Throws.ArgumentNullException);

        [TestCase("-d")]
        [TestCase("-dn")]
        [TestCase("-domain")]
        [TestCase("-domain-name")]
        [TestCase("-p")]
        [TestCase("-pn")]
        [TestCase("-provider")]
        [TestCase("-provider-name")]
        [TestCase("domain")]
        [TestCase("provider")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\t")]
        [TestCase("-")]
        [TestCase("/domain")]
        [TestCase("test.nucilandia.ro")]
        [TestCase(" --domain")]
        public void GivenAnInvalidOptionFormat_WhenParsing_ThenAnArgumentExceptionIsThrown(string option)
            => Assert.That(
                () => new InputSettings([option, "value"]),
                Throws.TypeOf<ArgumentException>().With.Message.EqualTo($"Invalid argument format: {option}"));

        [TestCase("--unknown")]
        [TestCase("--help")]
        [TestCase("--version")]
        [TestCase("--Domain")]
        [TestCase("--DOMAIN")]
        [TestCase("--Provider")]
        [TestCase("--PROVIDER")]
        [TestCase("--D")]
        [TestCase("--P")]
        [TestCase("--domain_name")]
        [TestCase("--provider_name")]
        [TestCase("--domainName")]
        [TestCase("--providerName")]
        [TestCase("--domain ")]
        [TestCase("--provider\t")]
        [TestCase("---domain")]
        [TestCase("---provider")]
        [TestCase("--")]
        [TestCase("--domain=test.nucilandia.ro")]
        [TestCase("--provider=cloudflare")]
        public void GivenAnUnknownOption_WhenParsing_ThenAnArgumentExceptionIsThrown(string option)
            => Assert.That(
                () => new InputSettings([option, "value"]),
                Throws.TypeOf<ArgumentException>().With.Message.EqualTo($"Unknown argument: {option[2..]}"));

        [TestCase("--d")]
        [TestCase("--dn")]
        [TestCase("--domain")]
        [TestCase("--domain-name")]
        [TestCase("--p")]
        [TestCase("--pn")]
        [TestCase("--provider")]
        [TestCase("--provider-name")]
        public void GivenAnOptionWithoutAValue_WhenParsing_ThenTheMissingParameterIsIdentified(string option)
            => Assert.That(
                () => new InputSettings([option]),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo(option[2..]));

        [TestCase("--d")]
        [TestCase("--dn")]
        [TestCase("--domain")]
        [TestCase("--domain-name")]
        [TestCase("--p")]
        [TestCase("--pn")]
        [TestCase("--provider")]
        [TestCase("--provider-name")]
        public void GivenATrailingOptionWithoutAValue_WhenParsing_ThenTheMissingParameterIsIdentified(
            string option)
            => Assert.That(
                () => new InputSettings(["--domain", "test.nucilandia.ro", option]),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo(option[2..]));

        [TestCase("extra")]
        [TestCase("test.nucilandia.ro")]
        [TestCase("cloudflare")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("-p")]
        public void GivenATrailingPositionalArgument_WhenParsing_ThenItIsRejected(string argument)
            => Assert.That(
                () => new InputSettings(["--domain", "test.nucilandia.ro", argument]),
                Throws.TypeOf<ArgumentException>().With.Message.EqualTo($"Invalid argument format: {argument}"));

        [TestCase("--domain", "--provider", "cloudflare")]
        [TestCase("--provider", "--domain", "test.nucilandia.ro")]
        public void GivenAnOptionConsumedAsAValue_WhenParsing_ThenTheRemainingPositionalValueIsRejected(
            string firstOption,
            string secondOption,
            string value)
            => Assert.That(
                () => new InputSettings([firstOption, secondOption, value]),
                Throws.TypeOf<ArgumentException>().With.Message.EqualTo($"Invalid argument format: {value}"));
    }
}