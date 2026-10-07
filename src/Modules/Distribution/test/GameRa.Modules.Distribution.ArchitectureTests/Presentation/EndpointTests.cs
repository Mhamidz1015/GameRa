using FluentAssertions;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Modules.Distribution.ArchitectureTests.Abstractions;
using NetArchTest.Rules;

namespace GameRa.Modules.Distribution.ArchitectureTests.Presentation;

public class EndpointTests : BaseTest
{
    [Fact]
    public void Endpoints_Should_NotBePublic()
    {
        Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IEndpoint))
            .Should()
            .NotBePublic()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void Endpoints_Should_BeSealed()
    {
        Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IEndpoint))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void Endpoints_Should_Exist()
    {
        Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IEndpoint))
            .GetTypes()
            .Should()
            .HaveCount(13);
    }
}
