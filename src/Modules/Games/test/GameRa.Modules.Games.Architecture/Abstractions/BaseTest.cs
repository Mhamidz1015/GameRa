using GameRa.Modules.Games.Infrastructure;
using GameRa.Modules.Games.Domain.Games;
using System.Reflection;

namespace GameRa.Modules.Games.Architecture_.Abstractions;

public abstract class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(Games.Application.AssemblyReference).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(Game).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(GamesModule).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Games.Presentation.AssemblyReference).Assembly;
}
