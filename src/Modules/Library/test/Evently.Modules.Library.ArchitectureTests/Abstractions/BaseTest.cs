using GameRa.Modules.Library.Domain.LibraryItems;
using GameRa.Modules.Library.Infrastructure;
using StackExchange.Redis;
using System.Reflection;

namespace GameRa.Modules.Library.ArchitectureTests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(Library.Application.AssemblyReference).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(LibraryItem).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(LibraryItemModule).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Library.Presentation.AssemblyReference).Assembly;
}
