using Bogus;
using Bogus.DataSets;
using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Domain.Games;
using GameRa.Modules.Games.UnitTests.Abstractions;

namespace GameRa.Modules.Games.UnitTests.Games;


public class GameTests : BaseTest
{
    // ─────────────────────────────────────────────
    // Create - Failure Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void Create_ShouldReturnFailure_WhenTitleIsEmpty()
    {
        // Arrange
        string title = string.Empty;

        // Act
        Result<Game> result = Game.Create(
            title,
            Faker.Lorem.Sentence(),
            Faker.Company.CompanyName(),
            DateTime.UtcNow.AddMonths(3),
            Faker.Random.Decimal(1, 200),
            Faker.Internet.Url());

        Game game = result.Value;

        // Assert
        result.Error.Should().Be(GameErrors.TitleIsEmpty);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenDescriptionIsEmpty()
    {
        // Arrange
        string description = string.Empty;

        // Act
        Result<Game> result = Game.Create(
            Faker.Commerce.ProductName(),
            description,
            Faker.Company.CompanyName(),
            DateTime.UtcNow.AddMonths(3),
            Faker.Random.Decimal(1, 200),
            Faker.Internet.Url());

        Game game = result.Value;

        // Assert
        result.Error.Should().Be(GameErrors.DescriptionIsEmpty);
    }


    [Fact]
    public void Create_ShouldReturnFailure_WhenBasePriceIsNegative()
    {
        // Arrange
        decimal basePrice = -1m;

        // Act
        Result<Game> result = Game.Create(
            Faker.Commerce.ProductName(),
            Faker.Lorem.Sentence(),
            Faker.Company.CompanyName(),
            DateTime.UtcNow.AddMonths(3),
            basePrice,
            Faker.Internet.Url());

        Game game = result.Value;

        // Assert
        result.Error.Should().Be(GameErrors.PriceCannotBeNegative);
    }

    // ─────────────────────────────────────────────
    // Create - Success Case
    // ─────────────────────────────────────────────

    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenGameCreated()
    {
        // Act
        Result<Game> result = Game.Create(
            Faker.Commerce.ProductName(),
            Faker.Lorem.Sentence(),
            Faker.Company.CompanyName(),
            DateTime.UtcNow.AddMonths(3),
            Faker.Random.Decimal(1, 200),
            Faker.Internet.Url());

        Game game = result.Value;

        // Assert
        GameAddedDomainEvent domainEvent =
            AssertDomainEventWasPublished<GameAddedDomainEvent>(result.Value);
        domainEvent.GameId.Should().Be(result.Value.Id);
    }

    // ─────────────────────────────────────────────
    // Release - Failure Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void Release_ShouldReturnFailure_WhenGameIsAlreadyReleased()
    {
        // Arrange
        Result<Game> result = CreateDefaultGame();

        Game game = result.Value;
        game.Release();

        // Act
        Result Releaseresult = game.Release();

        // Assert
        result.Error.Should().Be(GameErrors.Released);
    }

    [Fact]
    public void Release_ShouldReturnFailure_WhenGameIsDelisted()
    {
        // Arrange
        Result<Game> result = CreateDefaultGame();

        Game game = result.Value;
        game.Release();
        game.Delist();

        // Act
        Result Releaseresult = game.Release();

        // Assert
        result.Error.Should().Be(GameErrors.Released);
    }

    // ─────────────────────────────────────────────
    // Release - Success Case
    // ─────────────────────────────────────────────

    [Fact]
    public void Release_ShouldRaiseDomainEvent_WhenGameIsComingSoon()
    {
        // Arrange
        Result<Game> result = CreateDefaultGame();

        Game game = result.Value;

        // Act
        Result ReleaseResult = game.Release();

        // Assert
        GameReleasedDomainEvent domainEvent =
            AssertDomainEventWasPublished<GameReleasedDomainEvent>(game);
        domainEvent.GameId.Should().Be(game.Id);
    }
    // ─────────────────────────────────────────────
    // Delist - Failure Case
    // ─────────────────────────────────────────────

    [Fact]

    public void Delist_ShouldReturnFailure_WhenGameIsAlreadyDelisted()
    {
        // Arrange
        Result<Game> result = CreateDefaultGame();

        Game game = result.Value;
        game.Release();
        game.Delist();

        // Act
        Result Delistresult = game.Delist();

        // Assert
        result.Error.Should().Be(GameErrors.AlreadyDelisted);
    }

    // ─────────────────────────────────────────────
    // Delist - Success Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void Delist_ShouldRaiseDomainEvent_WhenGameIsReleased()
    {
        // Arrange
        Result<Game> result = CreateDefaultGame();

        Game game = result.Value;

        game.Release();

        // Act
        Result Delistresult = game.Delist();

        // Assert
        GameDelistedDomainEvent domainEvent =
            AssertDomainEventWasPublished<GameDelistedDomainEvent>(game);
        domainEvent.GameId.Should().Be(game.Id);
    }

    [Fact]
    public void Delist_ShouldRaiseDomainEvent_WhenGameIsComingSoon()
    {
        // Arrange
        Result<Game> result  = CreateDefaultGame();

        Game game = result.Value;

        // Act
        Result Delistresult = game.Delist();

        // Assert
        GameDelistedDomainEvent domainEvent =
            AssertDomainEventWasPublished<GameDelistedDomainEvent>(game);
        domainEvent.GameId.Should().Be(game.Id);
    }

    // ─────────────────────────────────────────────
    // Helper
    // ─────────────────────────────────────────────

    private Game CreateDefaultGame() =>
        Game.Create(
            Faker.Commerce.ProductName(),
            Faker.Lorem.Sentence(),
            Faker.Company.CompanyName(),
            DateTime.UtcNow.AddMonths(3),
            Faker.Random.Decimal(1, 200),
            Faker.Internet.Url()
        );
}