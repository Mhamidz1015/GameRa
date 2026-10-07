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
        libraryItemResult.Error.Should().Be(LibraryItemErrors.AlreadyArchived);
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
    // Unarchive
    // ─────────────────────────────────────────────

    [Fact]
    public void Unarchive_ShouldReturnFailure_WhenLibraryItemIsNotArchived()
    {
        // Arrange
        LibraryItem libraryItem = CreateDefaultLibraryItem();
        // Act
        Result result = libraryItem.Unarchive();
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(LibraryItemErrors.NotArchived);
    }
    [Fact]
    public void Unarchive_ShouldRaiseDomainEvent_WhenLibraryItemIsArchived()
    {
        // Arrange
        LibraryItem libraryItem = CreateDefaultLibraryItem();
        libraryItem.Archive();
        // Act
        Result result = libraryItem.Unarchive();
        // Assert
        result.IsSuccess.Should().BeTrue();
        libraryItem.IsArchived.Should().BeFalse();
        LibraryItemUnarchivedDomainEvent domainEvent =
            AssertDomainEventWasPublished<LibraryItemUnarchivedDomainEvent>(libraryItem);
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