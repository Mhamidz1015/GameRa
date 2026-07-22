using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Application.Games.GetGame;
using GameRa.Modules.Games.Domain.Games;
using GameRa.Modules.Games.IntegrationTests.Abstractions;

namespace GameRa.Modules.Games.IntegrationTests.Games;

public class GetGameTests : BaseIntegrationTest
{
    public GetGameTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenGameDoesNotExist()
    {
        // Arrange
        var query = new GetGameQuery(Guid.NewGuid());

        // Act
        Result<GameResponse> result = await Sender.Send(query);

        // Assert
        result.Error.Should().Be(GameErrors.NotFound(query.GameId));
    }

    [Fact]
    public async Task Should_ReturnGame_WhenGameExists()
    {
        // Arrange
        await CleanDatabaseAsync();

        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());

        Guid gameId = await Sender.AddGameAsync(categoryId);

        var query = new GetGameQuery(gameId);

        // Act
        Result<GameResponse> result = await Sender.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}
