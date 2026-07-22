using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Application.Games.GetGames;
using GameRa.Modules.Games.IntegrationTests.Abstractions;

namespace GameRa.Modules.Games.IntegrationTests.Games;

public class GetGamesTests : BaseIntegrationTest
{
    public GetGamesTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenGamesDoNotExist()
    {
        // Arrange
        await CleanDatabaseAsync();

        var query = new GetGamesQuery();

        // Act
        Result<IReadOnlyCollection<GameResponse>> result = await Sender.Send(query);

        // Assert
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_ReturnGames_WhenGamesExist()
    {
        // Arrange
        await CleanDatabaseAsync();

        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());

        await Sender.AddGameAsync(categoryId);
        await Sender.AddGameAsync(categoryId);

        var query = new GetGamesQuery();

        // Act
        Result<IReadOnlyCollection<GameResponse>> result = await Sender.Send(query);

        // Assert
        result.Value.Should().HaveCount(2);
    }
}
