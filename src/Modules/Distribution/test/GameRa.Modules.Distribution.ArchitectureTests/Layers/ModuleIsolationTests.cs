using System.Reflection;
using FluentAssertions;
using GameRa.Modules.Distribution.ArchitectureTests.Abstractions;
using NetArchTest.Rules;

namespace GameRa.Modules.Distribution.ArchitectureTests.Layers;

public class ModuleIsolationTests : BaseTest
{
    private const string DistributionPrefix = "GameRa.Modules.Distribution";

    private static readonly string[] OtherModules =
    [
        "GameRa.Modules.Users",
        "GameRa.Modules.Games",
        "GameRa.Modules.Store",
        "GameRa.Modules.Library",
        "GameRa.Modules.Discounts",
        "GameRa.Modules.Reviews"
    ];

    private static readonly Assembly[] DistributionAssemblies =
    [
        DomainAssembly,
        ApplicationAssembly,
        InfrastructureAssembly,
        PresentationAssembly,
        IntegrationEventsAssembly
    ];

    [Fact]
    public void DistributionModule_ShouldOnlyReference_OtherModulesIntegrationEvents()
    {
        var invalidReferences = DistributionAssemblies
            .SelectMany(assembly => assembly.GetReferencedAssemblies().Select(reference => (assembly, reference)))
            .Where(x => x.reference.Name!.StartsWith("GameRa.Modules.", StringComparison.Ordinal) &&
                        !x.reference.Name.StartsWith(DistributionPrefix, StringComparison.Ordinal) &&
                        !x.reference.Name.EndsWith(".IntegrationEvents", StringComparison.Ordinal))
            .Select(x => $"{x.assembly.GetName().Name} -> {x.reference.Name}")
            .ToList();

        invalidReferences.Should().BeEmpty();
    }

    [Fact]
    public void DistributionTypes_ShouldNotDependOn_OtherModulesInternals()
    {
        string[] allowedIntegrationEvents = OtherModules.Select(m => $"{m}.IntegrationEvents").ToArray();

        Types.InAssemblies(DistributionAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(allowedIntegrationEvents)
            .Should()
            .NotHaveDependencyOnAny(OtherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void DomainLayer_ShouldNotReference_AnyOtherLayer()
    {
        DomainAssembly.GetReferencedAssemblies()
            .Select(a => a.Name!)
            .Where(name => name.StartsWith("GameRa.", StringComparison.Ordinal))
            .Should()
            .BeEquivalentTo("GameRa.Common.Domain");
    }
}
