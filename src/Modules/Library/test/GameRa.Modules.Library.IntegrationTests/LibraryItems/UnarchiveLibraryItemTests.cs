using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.CheckGameOwnership;
using GameRa.Modules.Library.Application.LibraryItems.GetUserLibrary;
using GameRa.Modules.Library.Application.LibraryItems.UnarchiveLibraryItem;
using GameRa.Modules.Library.Domain.LibraryItems;
using GameRa.Modules.Library.IntegrationTests.Abstractions;
using Xunit;

namespace GameRa.Modules.Library.IntegrationTests.LibraryItems;

public class UnarchiveLibraryItemTests : BaseIntegrationTest
{
    public UnarchiveLibraryItemTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task UnarchiveLibraryItem_ShouldRestoreGameToActiveLibrary_WhenArchived()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Guid gameId = Guid.NewGuid();

        await Sender.AddGameToLibraryAsync(userId, gameId);
        await Sender.ArchiveLibraryItemAsync(userId, gameId);

        // Act
        Result result = await Sender.Send(new UnarchiveLibraryItemCommand(userId, gameId));

        // Assert
        result.IsSuccess.Should().BeTrue();

        Result<IReadOnlyCollection<LibraryItemResponse>> active =
            await Sender.Send(new GetUserLibraryQuery(userId, LibraryFilter.Active));

        active.Value.Should().ContainSingle(l => l.GameId == gameId);
    }

    [Fact]
    public async Task UnarchiveLibraryItem_ShouldKeepOwnership_WhileArchivedAndAfterRestore()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Guid gameId = Guid.NewGuid();

        await Sender.AddGameToLibraryAsync(userId, gameId);
        await Sender.ArchiveLibraryItemAsync(userId, gameId);

        // Act
        Result<bool> whileArchived = await Sender.Send(new CheckGameOwnershipQuery(userId, gameId));
        await Sender.Send(new UnarchiveLibraryItemCommand(userId, gameId));
        Result<bool> afterRestore = await Sender.Send(new CheckGameOwnershipQuery(userId, gameId));

        // Assert
        whileArchived.Value.Should().BeTrue();
        afterRestore.Value.Should().BeTrue();
    }

    [Fact]
    public async Task UnarchiveLibraryItem_ShouldReturnFailure_WhenNotArchived()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Guid gameId = Guid.NewGuid();

        await Sender.AddGameToLibraryAsync(userId, gameId);

        // Act
        Result result = await Sender.Send(new UnarchiveLibraryItemCommand(userId, gameId));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(LibraryItemErrors.NotArchived);
    }

    [Fact]
    public async Task UnarchiveLibraryItem_ShouldReturnFailure_WhenLibraryItemNotFound()
    {
        // Act
        Result result = await Sender.Send(
            new UnarchiveLibraryItemCommand(Guid.NewGuid(), Guid.NewGuid()));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(LibraryItemErrors.NotFound);
    }
}