using GameRa.ArchitectureTests.Abstractions;
using GameRa.Modules.Games.Infrastructure;
using GameRa.Modules.Library.Infrastructure;
using GameRa.Modules.Games.Domain.Games;
using GameRa.Modules.Store.Domain.Orders;
using GameRa.Modules.Store.Infrastructure;
using GameRa.Modules.Users.Domain.Users;
using GameRa.Modules.Users.Infrastructure;
using Microsoft.Extensions.DependencyModel;
using NetArchTest.Rules;
using System.Reflection;
using GameRa.Modules.Discounts.Infrastructure;
using GameRa.Modules.Discounts.Domain;
using GameRa.Modules.Reviews.Domain;
using GameRa.Modules.Reviews.Infrastructure;

namespace GameRa.ArchitectureTests.Layers;

public class ModuleTests : BaseTest
{
    [Fact]
    public void UsersModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules = [GamesNamespace, StoreNamespace, LibraryNamespace, DiscountsNamespace, ReviewsNamespace];
        string[] integrationEventsModules =
        [
            GamesIntegrationEventsNamespace,
            StoreIntegrationEventsNamespace,
            LibraryIntegrationEventsNamespace,
            DiscountsIntegrationEventsNamespace,
            ReviewsIntegrationEventsNamespace,

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
        string[] otherModules = [UsersNamespace, StoreNamespace, LibraryNamespace, DiscountsNamespace, ReviewsNamespace];
        string[] integrationEventsModules =
        [
            UsersIntegrationEventsNamespace,
            StoreIntegrationEventsNamespace,
            LibraryIntegrationEventsNamespace,
            DiscountsIntegrationEventsNamespace,
            ReviewsIntegrationEventsNamespace,
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
        string[] otherModules = [GamesNamespace, UsersNamespace, LibraryNamespace, DiscountsNamespace, ReviewsNamespace];
        string[] integrationEventsModules =
        [
            GamesIntegrationEventsNamespace,
            UsersIntegrationEventsNamespace,
            LibraryIntegrationEventsNamespace,
            DiscountsIntegrationEventsNamespace,
            ReviewsIntegrationEventsNamespace,

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

    [Fact]
    public void LibraryItemsModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules = [UsersNamespace, GamesNamespace, StoreNamespace, DiscountsNamespace, ReviewsNamespace];
        string[] integrationEventsModulesAndPublicsApi = 
            [
            UsersIntegrationEventsNamespace,
            GamesIntegrationEventsNamespace,
            StoreIntegrationEventsNamespace,
            DiscountsIntegrationEventsNamespace,
            ReviewsIntegrationEventsNamespace,
            ];

        List<Assembly> LibraryAssemblies =
        [
            typeof(Library).Assembly,
            Modules.Library.Application.AssemblyReference.Assembly,
            Modules.Library.Presentation.AssemblyReference.Assembly,
            typeof(LibraryItemModule).Assembly
        ];

        Types.InAssemblies(LibraryAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModulesAndPublicsApi)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void DiscountsModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules = [UsersNamespace, GamesNamespace, StoreNamespace, LibraryNamespace, ReviewsNamespace];
        string[] integrationEventsModules = [
            UsersIntegrationEventsNamespace,
            GamesIntegrationEventsNamespace,
            StoreIntegrationEventsNamespace,
            LibraryIntegrationEventsNamespace,
            ReviewsIntegrationEventsNamespace
            ];

        List<Assembly> DiscountsAssemblies =
        [
            typeof(Discount).Assembly,
            Modules.Discounts.Application.AssemblyReference.Assembly,
            Modules.Discounts.Presentation.AssemblyReference.Assembly,
            typeof(DiscountsModule).Assembly
        ];

        Types.InAssemblies(DiscountsAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void ReviewsModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules = [UsersNamespace, GamesNamespace, StoreNamespace, LibraryNamespace, DiscountsNamespace];
        string[] integrationEventsModules = [
            UsersIntegrationEventsNamespace,
            GamesIntegrationEventsNamespace,
            StoreIntegrationEventsNamespace,
            LibraryIntegrationEventsNamespace,
            DiscountsIntegrationEventsNamespace,
            ];

        List<Assembly> ReviewsAssemblies =
        [
            typeof(Review).Assembly,
            Modules.Reviews.Application.AssemblyReference.Assembly,
            Modules.Reviews.Presentation.AssemblyReference.Assembly,
            typeof(ReviewsModule).Assembly
        ];

        Types.InAssemblies(ReviewsAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }
}