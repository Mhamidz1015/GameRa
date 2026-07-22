using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Application.Games.ReleaseGame;
using GameRa.Modules.Games.Domain.Games;
using GameRa.Modules.Games.IntegrationTests.Abstractions;

namespace GameRa.Modules.Games.IntegrationTests.Games;

public class ReleaseGameTests : BaseIntegrationTest
{
    public ReleaseGameTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenGameDoesNotExist()
    {
        // Arrange
        var gameId = Guid.NewGuid();

        var command = new ReleaseGameCommand(gameId);

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.Error.Should().Be(GameErrors.NotFound(gameId));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenGameIsReleased()
    {
        // Arrange
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());
        Guid gameId = await Sender.AddGameAsync(categoryId);

        var command = new ReleaseGameCommand(gameId);

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
