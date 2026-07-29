using Bogus;
using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.GetUserLibrary;
using GameRa.Modules.Library.IntegrationTests.Abstractions;
using Xunit;

namespace GameRa.Modules.Library.IntegrationTests.LibraryItems;

public class GetUserLibraryTests : BaseIntegrationTest
{
    protected GetUserLibraryTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetUserLibrary_ShouldReturnOnlyActiveItems_WhenFilterIsActive()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Guid activeGameId = Guid.NewGuid();
        Guid archivedGameId = Guid.NewGuid();

        await Sender.AddGameToLibraryAsync(userId, activeGameId);
        await Sender.AddGameToLibraryAsync(userId, archivedGameId);
        await Sender.ArchiveLibraryItemAsync(userId, archivedGameId);

        var query = new GetUserLibraryQuery(userId, LibraryFilter.Active);

        // Act
        Result<IReadOnlyCollection<LibraryItemResponse>> result = await Sender.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainSingle(item => item.GameId == activeGameId);
        result.Value.Should().NotContain(item => item.GameId == archivedGameId);
    }

    [Fact]
    public async Task GetUserLibrary_ShouldReturnAllItems_WhenFilterIsAll()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Guid gameId1 = Guid.NewGuid();
        Guid gameId2 = Guid.NewGuid();

        await Sender.AddGameToLibraryAsync(userId, gameId1);
        await Sender.AddGameToLibraryAsync(userId, gameId2);
        await Sender.ArchiveLibraryItemAsync(userId, gameId2);

        var query = new GetUserLibraryQuery(userId, LibraryFilter.All);

        // Act
        Result<IReadOnlyCollection<LibraryItemResponse>> result = await Sender.Send(query);

        // Assert
        result.Value.Should().HaveCount(2);
    }
}