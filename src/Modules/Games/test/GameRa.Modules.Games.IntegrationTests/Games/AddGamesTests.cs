using Bogus;
using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Application.Games.AddGame;
using GameRa.Modules.Games.Domain.Categories;
using GameRa.Modules.Games.IntegrationTests.Abstractions;

namespace GameRa.Modules.Games.IntegrationTests.Games;

public class AddGamesTests : BaseIntegrationTest
{
    public AddGamesTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        var command = new AddGameCommand(
            categoryId,
                Faker.Commerce.ProductName(),
                Faker.Lorem.Sentence(),
                Faker.Company.CompanyName(),
                DateTime.UtcNow.AddMonths(-1),
                Faker.Random.Decimal(1, 200),
                Faker.Internet.Url());

        // Act
        Result<Guid> result = await Sender.Send(command);

        // Assert
        result.Error.Should().Be(CategoryErrors.NotFound(categoryId));
    }

    [Fact]
    public async Task Should_AddGame_WhenCommandIsValid()
    {
        // Arrange
        await CleanDatabaseAsync();
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());

        var command = new AddGameCommand(
                categoryId,
                Faker.Commerce.ProductName(),
                Faker.Lorem.Sentence(),
                Faker.Company.CompanyName(),
                DateTime.UtcNow.AddMonths(-1),
                Faker.Random.Decimal(1, 200),
                Faker.Internet.Url());

        // Act
        Result<Guid> result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }
}
