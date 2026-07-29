using Bogus;
using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.CheckGameOwnership;
using GameRa.Modules.Library.IntegrationTests.Abstractions;
using Xunit;

namespace GameRa.Modules.Library.IntegrationTests.LibraryItems;

public class CheckGameOwnershipTests : BaseIntegrationTest
{
    protected CheckGameOwnershipTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CheckGameOwnership_ShouldReturnTrue_WhenUserOwnsActiveGame()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Guid gameId = Guid.NewGuid();

        await Sender.AddGameToLibraryAsync(userId, gameId);

        var query = new CheckGameOwnershipQuery(userId, gameId);

        // Act
        Result<bool> result = await Sender.Send(query);

        // Assert
        result.Value.Should().BeTrue();
    }

    [Fact]
    public async Task CheckGameOwnership_ShouldReturnFalse_WhenUserDoesNotOwnGame()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Guid gameId = Guid.NewGuid();

        var query = new CheckGameOwnershipQuery(userId, gameId);

        // Act
        Result<bool> result = await Sender.Send(query);

        // Assert
        result.Value.Should().BeFalse();
    }

    [Fact]
    public async Task CheckGameOwnership_ShouldReturnFalse_WhenGameIsArchived()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Guid gameId = Guid.NewGuid();

        await Sender.AddGameToLibraryAsync(userId, gameId);
        await Sender.ArchiveLibraryItemAsync(userId, gameId);

        var query = new CheckGameOwnershipQuery(userId, gameId);

        // Act
        Result<bool> result = await Sender.Send(query);

        // Assert
        result.Value.Should().BeFalse();
    }
}