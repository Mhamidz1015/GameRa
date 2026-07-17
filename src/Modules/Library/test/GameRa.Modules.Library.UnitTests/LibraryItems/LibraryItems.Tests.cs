using Bogus;
using GameRa.Common.Domain.Abstractions;
using FluentAssertions;
using GameRa.Modules.Library.Domain.LibraryItems;
using GameRa.Modules.Library.UnitTests.Abstractions;

namespace GameRa.Modules.Library.UnitTests.LibraryItems;

public class LibraryItemsTests : BaseTest
{
    // ─────────────────────────────────────────────
    // Create
    // ─────────────────────────────────────────────

    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenLibraryItemCreated()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Guid gameId = Guid.NewGuid();
        string gameTitleSnapshot = Faker.Commerce.ProductName();

        // Act
        LibraryItem libraryItem = LibraryItem.Create(userId, gameId, gameTitleSnapshot);



        // Assert
        LibraryItemCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<LibraryItemCreatedDomainEvent>(libraryItem);
        domainEvent.LibraryId.Should().Be(libraryItem.Id);
    }

    // ─────────────────────────────────────────────
    // Archive - Failure Case
    // ─────────────────────────────────────────────

    [Fact]
    public void Archive_ShouldReturnFailure_WhenLibraryItemAlreadyArchived()
    {
        // Arrange
        Result<LibraryItem> result = CreateDefaultLibraryItem();

        LibraryItem libraryItem = result.Value;
        libraryItem.Archive();

        // Act
        Result libraryItemResult = libraryItem.Archive();

        // Assert
        result.Error.Should().Be(LibraryItemErrors.AlreadyArchived);
    }

    // ─────────────────────────────────────────────
    // Archive - Success Case
    // ─────────────────────────────────────────────

    [Fact]
    public void Archive_ShouldRaiseDomainEvent_WhenLibraryItemIsNotArchived()
    {
        // Arrange
        Result<LibraryItem> result = CreateDefaultLibraryItem();

        LibraryItem libraryItem = result.Value;
        libraryItem.Archive();

        // Act
        Result libraryItemResult = libraryItem.Archive();

        // Assert
        LibraryItemArchivedDomainEvent domainEvent =
            AssertDomainEventWasPublished<LibraryItemArchivedDomainEvent>(libraryItem);
        domainEvent.LibraryId.Should().Be(libraryItem.Id);
    }

    // ─────────────────────────────────────────────
    // Helper
    // ─────────────────────────────────────────────

    private LibraryItem CreateDefaultLibraryItem() =>
        LibraryItem.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Faker.Commerce.ProductName());
}