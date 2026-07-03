using System.Reflection;
using GameRa.ArchitectureTests.Abstractions;
using GameRa.Modules.Games.Infrastructure;
using GameRa.Modules.Store.Domain.Games;
using GameRa.Modules.Store.Domain.Orders;
using GameRa.Modules.Store.Infrastructure;
using GameRa.Modules.Users.Domain.Users;
using GameRa.Modules.Users.Infrastructure;
using NetArchTest.Rules;

namespace GameRa.ArchitectureTests.Layers;

public class ModuleTests : BaseTest
{
    [Fact]
    public void UsersModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules = [GamesNamespace, StoreNamespace];
        string[] integrationEventsModules =
        [
            GamesIntegrationEventsNamespace,
            StoreIntegrationEventsNamespace
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
        string[] otherModules = [UsersNamespace, StoreNamespace];
        string[] integrationEventsModules =
        [
            UsersIntegrationEventsNamespace,
            StoreIntegrationEventsNamespace
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
        string[] otherModules = [GamesNamespace, UsersNamespace];
        string[] integrationEventsModules =
        [
            GamesIntegrationEventsNamespace,
            UsersIntegrationEventsNamespace,
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
}