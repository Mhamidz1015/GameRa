using GameRa.Modules.Discounts.Domain;
using GameRa.Modules.Discounts.Infrastructure;
using System.Reflection;

namespace GameRa.Modules.Discounts.ArchitectureTests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(Discounts.Application.AssemblyReference).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(Discount).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(DiscountsModule).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Discounts.Presentation.AssemblyReference).Assembly;
}
