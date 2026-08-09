using GameRa.Modules.Reviews.Domain;
using GameRa.Modules.Reviews.Infrastructure;
using System.Reflection;

namespace GameRa.Modules.Reviews.ArchitectureTests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(Reviews.Application.AssemblyReference).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(Review).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(ReviewsModule).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Reviews.Presentation.AssemblyReference).Assembly;
}
