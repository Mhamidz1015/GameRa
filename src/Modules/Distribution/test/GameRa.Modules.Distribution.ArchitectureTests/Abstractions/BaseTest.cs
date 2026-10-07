using System.Reflection;
using GameRa.Modules.Distribution.Domain.GameVersions;
using GameRa.Modules.Distribution.Infrastructure;
using GameRa.Modules.Distribution.IntegrationEvents;

namespace GameRa.Modules.Distribution.ArchitectureTests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(Distribution.Application.AssemblyReference).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(GameVersion).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(DistributionModule).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Distribution.Presentation.AssemblyReference).Assembly;

    protected static readonly Assembly IntegrationEventsAssembly = typeof(GameVersionPublishedIntegrationEvent).Assembly;
}
