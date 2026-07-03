using GameRa.Modules.Store.Domain.Orders;
using GameRa.Modules.Store.Infrastructure;
using System.Reflection;

namespace GameRa.Modules.Store.Architecture_.Abstractions;

public abstract class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(Store.Application.AssemblyReference).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(Order).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(StoreModule).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Store.Presentation.AssemblyReference).Assembly;
}
