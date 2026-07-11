using GameRa.ArchitectureTests.Abstractions;
using GameRa.Modules.Games.Infrastructure;
using GameRa.Modules.Library.Infrastructure;
using GameRa.Modules.Store.Domain.Games;
using GameRa.Modules.Store.Domain.Orders;
using GameRa.Modules.Store.Infrastructure;
using GameRa.Modules.Users.Domain.Users;
using GameRa.Modules.Users.Infrastructure;
using Microsoft.Extensions.DependencyModel;
using NetArchTest.Rules;
using System.Reflection;

namespace GameRa.ArchitectureTests.Layers;

public class ModuleTests : BaseTest
{
    [Fact]
    public void UsersModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules = [GamesNamespace, StoreNamespace, LibraryNamespace];
        string[] integrationEventsModules =
        [
            GamesIntegrationEventsNamespace,
            StoreIntegrationEventsNamespace,
            LibraryIntegrationEventsNamespace

        ];

        List<Assembly> usersAssemblies =
        [
            typeof(User).Assembly,
            Modules.Users.Application.AssemblyReference.Assembly,
            Modules.Users.Presentation.AssemblyReference.Assembly,
            typeof(UsersModule).Assembly
        ];

        Types.InAssemblies(usersAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void GamesModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules = [UsersNamespace, StoreNamespace, LibraryNamespace];
        string[] integrationEventsModules =
        [
            UsersIntegrationEventsNamespace,
            StoreIntegrationEventsNamespace,
            LibraryIntegrationEventsNamespace
        ];

        List<Assembly> gamesAssemblies =
        [
            typeof(Game).Assembly,
            Modules.Games.Application.AssemblyReference.Assembly,
            Modules.Games.Presentation.AssemblyReference.Assembly,
            typeof(GamesModule).Assembly
        ];

        Types.InAssemblies(gamesAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void StoreModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules = [GamesNamespace, UsersNamespace, LibraryNamespace];
        string[] integrationEventsModules =
        [
            GamesIntegrationEventsNamespace,
            UsersIntegrationEventsNamespace,
            LibraryIntegrationEventsNamespace

        ];

        List<Assembly> StoreAssemblies =
        [
            typeof(Order).Assembly,
            Modules.Store.Application.AssemblyReference.Assembly,
            Modules.Store.Presentation.AssemblyReference.Assembly,
            typeof(StoreModule).Assembly
        ];

        Types.InAssemblies(StoreAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }

    public void LibraryItemsModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules = [UsersNamespace, GamesNamespace, StoreNamespace];
        string[] integrationEventsModules = [
            UsersIntegrationEventsNamespace,
            GamesIntegrationEventsNamespace,
            StoreIntegrationEventsNamespace];

        List<Assembly> attendanceAssemblies =
        [
            typeof(Library).Assembly,
            Modules.Library.Application.AssemblyReference.Assembly,
            Modules.Library.Presentation.AssemblyReference.Assembly,
            typeof(LibraryItemModule).Assembly
        ];

        Types.InAssemblies(attendanceAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }
}