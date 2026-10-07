using FluentAssertions;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Modules.Distribution.ArchitectureTests.Abstractions;
using NetArchTest.Rules;

namespace GameRa.Modules.Distribution.ArchitectureTests.IntegrationEvents;

public class IntegrationEventTests : BaseTest
{
    [Fact]
    public void IntegrationEvents_Should_BeSealed()
    {
        Types.InAssembly(IntegrationEventsAssembly)
            .That()
            .Inherit(typeof(IntegrationEvent))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void IntegrationEvents_ShouldHave_IntegrationEventPostfix()
    {
        Types.InAssembly(IntegrationEventsAssembly)
            .That()
            .Inherit(typeof(IntegrationEvent))
            .Should()
            .HaveNameEndingWith("IntegrationEvent")
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void IntegrationEventsProject_ShouldOnlyReference_CommonApplication()
    {
        IntegrationEventsAssembly.GetReferencedAssemblies()
            .Select(a => a.Name!)
            .Where(name => name.StartsWith("GameRa.", StringComparison.Ordinal))
            .Should()
            .BeEquivalentTo("GameRa.Common.Application");
    }
}
