using System;
using System.Linq;
using ReflectionAssembly = System.Reflection.Assembly;
using ReflectionType = System.Type;

using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.NUnit;

using NUnit.Framework;

using DynamicDnsUpdater.Client;
using DynamicDnsUpdater.Client.Configuration;
using DynamicDnsUpdater.Client.Logging;
using DynamicDnsUpdater.Client.Requests;

using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace DynamicDnsUpdater.Client.ArchitectureTests
{
    [TestFixture]
    public sealed class ProductionArchitectureTests
    {
        static readonly ReflectionAssembly ProductionAssembly = typeof(Program).Assembly;
        static readonly Architecture Architecture = new ArchLoader()
            .LoadAssemblies(ProductionAssembly)
            .Build();

        [Test]
        public void GivenProductionAssembly_WhenLoaded_ThenItContainsExpectedTypes()
        {
            ReflectionType[] expectedTypes =
            [
                typeof(Program),
                typeof(IDnsRecordService),
                typeof(DnsRecordService),
                typeof(InputSettings),
                typeof(ApiSettings),
                typeof(PutDnsRecordRequest),
                typeof(MyOperation),
                typeof(MyLogInfoKey)
            ];

            foreach (ReflectionType expectedType in expectedTypes)
            {
                Assert.That(
                    ProductionAssembly.GetType(expectedType.FullName!),
                    Is.Not.Null,
                    expectedType.FullName);
            }
        }

        [Test]
        public void GivenProductionAssembly_WhenAnalysed_ThenProductionTypesResideInTheProductionAssembly()
        {
            Assert.That(
                Architecture.Types.Where(type => string.Equals(type.Assembly.Name, ProductionAssembly.GetName().Name, StringComparison.Ordinal)),
                Is.Not.Empty);
        }

        [Test]
        public void GivenProductionAssembly_WhenAnalysed_ThenConfigurationTypesStayInConfigurationNamespace()
            => Classes().That().Are(typeof(ApiSettings), typeof(InputSettings)).Should().ResideInNamespace("DynamicDnsUpdater.Client.Configuration").Check(Architecture);

        [Test]
        public void GivenProductionAssembly_WhenAnalysed_ThenRequestTypesStayInRequestsNamespace()
            => Classes().That().Are(typeof(PutDnsRecordRequest)).Should().ResideInNamespace("DynamicDnsUpdater.Client.Requests").Check(Architecture);

        [Test]
        public void GivenProductionAssembly_WhenAnalysed_ThenLoggingTypesStayInLoggingNamespace()
            => Classes().That().Are(typeof(MyOperation), typeof(MyLogInfoKey)).Should().ResideInNamespace("DynamicDnsUpdater.Client.Logging").Check(Architecture);

        [Test]
        public void GivenProductionAssembly_WhenAnalysed_ThenConfigurationTypesDoNotDependOnRequestTypes()
        {
            Classes().That().Are(typeof(ApiSettings), typeof(InputSettings)).Should()
                .NotDependOnAny(Classes().That().Are(typeof(PutDnsRecordRequest)))
                .Check(Architecture);
        }

        [Test]
        public void GivenProductionAssembly_WhenAnalysed_ThenRequestTypesDoNotDependOnConfigurationTypes()
        {
            Classes().That().Are(typeof(PutDnsRecordRequest)).Should()
                .NotDependOnAny(Classes().That().Are(typeof(ApiSettings), typeof(InputSettings)))
                .Check(Architecture);
        }

        [Test]
        public void GivenProductionAssembly_WhenAnalysed_ThenServiceContractIsAnInterface()
        {
            Interfaces().That().Are(typeof(IDnsRecordService)).Should().BePublic().Check(Architecture);
        }

        [Test]
        public void GivenProductionAssembly_WhenAnalysed_ThenServiceImplementationImplementsContract()
        {
            Classes().That().ImplementInterface(typeof(IDnsRecordService)).Should().Be(typeof(DnsRecordService)).Check(Architecture);
        }

        [Test]
        public void GivenProductionAssembly_WhenAnalysed_ThenProductionTypesDoNotReferenceTestAssemblies()
        {
            string[] testAssemblyNames = ProductionAssembly
                .GetReferencedAssemblies()
                .Where(assembly => assembly.Name is not null && assembly.Name.EndsWith("Tests", StringComparison.Ordinal))
                .Select(assembly => assembly.Name!)
                .ToArray();

            Assert.That(testAssemblyNames, Is.Empty);
        }
    }
}
