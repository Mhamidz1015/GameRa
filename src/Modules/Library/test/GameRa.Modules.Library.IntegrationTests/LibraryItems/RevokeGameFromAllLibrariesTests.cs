using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.CheckGameOwnership;
using GameRa.Modules.Library.Application.LibraryItems.RevokeGameFromAllLibraries;
using GameRa.Modules.Library.IntegrationTests.Abstractions;
using Xunit;

namespace GameRa.Modules.Library.IntegrationTests.LibraryItems;

public class RevokeGameFromAllLibrariesTests : BaseIntegrationTest
{
    public RevokeGameFromAllLibrariesTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task RevokeGame_ShouldRemoveGameFromEveryLibrary_AndKeepOtherGames()
    {
        // Arrange
        Guid firstUserId = Guid.NewGuid();
        Guid secondUserId = Guid.NewGuid();
        Guid revokedGameId = Guid.NewGuid();
        Guid otherGameId = Guid.NewGuid();

        await Sender.AddGameToLibraryAsync(firstUserId, revokedGameId);
        await Sender.AddGameToLibraryAsync(secondUserId, revokedGameId);
        await Sender.ArchiveLibraryItemAsync(secondUserId, revokedGameId);
        await Sender.AddGameToLibraryAsync(firstUserId, otherGameId);

        // Act
        Result result = await Sender.Send(new RevokeGameFromAllLibrariesCommand(revokedGameId));

        // Assert
        result.IsSuccess.Should().BeTrue();

        (await Sender.Send(new CheckGameOwnershipQuery(firstUserId, revokedGameId))).Value.Should().BeFalse();
        (await Sender.Send(new CheckGameOwnershipQuery(secondUserId, revokedGameId))).Value.Should().BeFalse();
        (await Sender.Send(new CheckGameOwnershipQuery(firstUserId, otherGameId))).Value.Should().BeTrue();
    }

    [Fact]
    public async Task RevokeGame_ShouldSucceed_WhenNobodyOwnsTheGame()
    {
        // Act
        Result result = await Sender.Send(new RevokeGameFromAllLibrariesCommand(Guid.NewGuid()));

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}