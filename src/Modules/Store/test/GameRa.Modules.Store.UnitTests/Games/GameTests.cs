using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Domain.Games;
using GameRa.Modules.Store.UnitTests.Abstractions;

namespace GameRa.Modules.Store.UnitTests.Games;

public class GameTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnValue_WhenGameIsAdded()
    {
        //Act
        Result<Game> game = Game.Create(
            Guid.NewGuid(),
            Faker.Commerce.ProductName(),
            Faker.Lorem.Sentence(),
            Faker.Company.CompanyName(),
            Faker.Random.Decimal(1, 200),
            Faker.Internet.Url());

        //Assert
        game.Value.Should().NotBeNull();
    }

    [Fact]
    public void Delist_ShouldRaiseDomainEvent_WhenGameIsDelisted()
    {
        //Arrange
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Game> game = Game.Create(
            Guid.NewGuid(),
            Faker.Commerce.ProductName(),
            Faker.Lorem.Sentence(),
            Faker.Company.CompanyName(),
            Faker.Random.Decimal(1, 200),
            Faker.Internet.Url());

        //Act
        game.Value.Delist();

        //Assert
        GameDelistDomainEvent domainEvent =
            AssertDomainEventWasPublished<GameDelistDomainEvent>(game.Value);

        domainEvent.GameId.Should().Be(game.Value.Id);
    }

    [Fact]
    public void PaymentsRefunded_ShouldRaiseDomainEvent_WhenPaymentsAreRefunded()
    {
        //Arrange
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Game> game = Game.Create(
            Guid.NewGuid(),
            Faker.Commerce.ProductName(),
            Faker.Lorem.Sentence(),
            Faker.Company.CompanyName(),
            Faker.Random.Decimal(1, 200),
            Faker.Internet.Url());

        //Act
        game.Value.PaymentsRefunded();

        //Assert
        GamePaymentsRefundedDomainEvent domainEvent =
            AssertDomainEventWasPublished<GamePaymentsRefundedDomainEvent>(game.Value);

        domainEvent.GameId.Should().Be(game.Value.Id);

    }
}
