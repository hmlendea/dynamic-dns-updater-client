using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

namespace DynamicDnsUpdater.Client.ArchitectureTests
{
    [TestFixture]
    public sealed class SourceFileConventionTests
    {
        [Test]
        public void GivenProductionSources_WhenAnalysed_ThenEachFileContainsOneTypeMatchingItsName()
        {
            Assembly assembly = typeof(SourceFileConventionTests).Assembly;
            string[] resourceNames = assembly
                .GetManifestResourceNames()
                .Where(name => name.StartsWith("ProductionSources/", StringComparison.Ordinal))
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToArray();

            Assert.That(resourceNames, Is.Not.Empty, "No production source resources were found.");

            foreach (string resourceName in resourceNames)
            {
                using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
                using StreamReader reader = new(stream);
                string source = reader.ReadToEnd();
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);
                SyntaxNode root = syntaxTree.GetRoot();
                List<SyntaxNode> declarations = root
                    .DescendantNodes()
                    .Where(node => node is BaseTypeDeclarationSyntax or DelegateDeclarationSyntax)
                    .ToList();

                string fileName = resourceName[(resourceName.LastIndexOf('/') + 1)..];
                string expectedName = Path.GetFileNameWithoutExtension(fileName);
                string actualPath = resourceName["ProductionSources/".Length..];

                Assert.That(
                    declarations,
                    Has.Count.EqualTo(1),
                    $"{actualPath} must contain exactly one declared type.");
                Assert.That(
                    GetTypeIdentifier(declarations[0]),
                    Is.EqualTo(expectedName),
                    $"{actualPath} must contain a type named {expectedName}.");
            }
        }

        private static string GetTypeIdentifier(SyntaxNode declaration)
        {
            string identifier = declaration switch
            {
                BaseTypeDeclarationSyntax typeDeclaration => typeDeclaration.Identifier.ValueText,
                DelegateDeclarationSyntax delegateDeclaration => delegateDeclaration.Identifier.ValueText,
                _ => string.Empty
            };

            return identifier;
        }
    }
}
