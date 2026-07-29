using Bogus;
using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.AddGameToLibrary;
using GameRa.Modules.Library.IntegrationTests.Abstractions;

namespace GameRa.Modules.Library.IntegrationTests.LibraryItems;

public class AddGameToLibraryTests : BaseIntegrationTest
{
    protected AddGameToLibraryTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task AddGameToLibrary_ShouldSucceed_WhenGameNotAlreadyOwned()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Guid gameId = Guid.NewGuid();
        string gameTitleSnapshot = Faker.Commerce.ProductName();

        var command = new AddGameToLibraryCommand(userId, gameId, gameTitleSnapshot);

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task AddGameToLibrary_ShouldSucceed_WhenGameAlreadyOwned()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Guid gameId = Guid.NewGuid();
        string gameTitleSnapshot = Faker.Commerce.ProductName();

        await Sender.AddGameToLibraryAsync(userId, gameId);

        var command = new AddGameToLibraryCommand(userId, gameId, gameTitleSnapshot);

        // Act 
        Result result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}