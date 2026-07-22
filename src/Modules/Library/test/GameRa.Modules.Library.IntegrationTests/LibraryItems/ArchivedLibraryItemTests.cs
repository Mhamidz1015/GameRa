using Bogus;
using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.ArchivedLibraryItem;
using GameRa.Modules.Library.Domain.LibraryItems;
using GameRa.Modules.Library.IntegrationTests.Abstractions;
using Xunit;

namespace GameRa.Modules.Library.IntegrationTests.LibraryItems;

[Collection(nameof(IntegrationTestCollection))]
public class ArchivedLibraryItemTests : BaseIntegrationTest
{
    protected ArchivedLibraryItemTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ArchiveLibraryItem_ShouldSucceed_WhenLibraryItemExists()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Guid gameId = Guid.NewGuid();
        string gameTitleSnapshot = Faker.Commerce.ProductName();

        await Sender.AddGameToLibraryAsync(userId, gameId);

        var command = new ArchivedLibraryItemCommand(userId, gameId);

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ArchiveLibraryItem_ShouldReturnFailure_WhenLibraryItemNotFound()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Guid gameId = Guid.NewGuid();

        var command = new ArchivedLibraryItemCommand(userId, gameId);

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(LibraryItemErrors.NotFound);
    }

    [Fact]
    public async Task ArchiveLibraryItem_ShouldReturnFailure_WhenAlreadyArchived()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Guid gameId = Guid.NewGuid();
        string gameTitleSnapshot = Faker.Commerce.ProductName();

        await Sender.AddGameToLibraryAsync(userId, gameId);
        await Sender.ArchiveLibraryItemAsync(userId, gameId);

        var command = new ArchivedLibraryItemCommand(userId, gameId);

        // Act 
        Result result = await Sender.Send(command);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}