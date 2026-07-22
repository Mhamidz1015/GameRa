using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Application.Games.DelistGame;
using GameRa.Modules.Games.Domain.Games;
using GameRa.Modules.Games.IntegrationTests.Abstractions;
using NSubstitute;

namespace GameRa.Modules.Games.IntegrationTests.Games;

public class DelistGameTests : BaseIntegrationTest
{
    public DelistGameTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenGameDoesNotExist()
    {
        // Arrange
        var gameId = Guid.NewGuid();

        var command = new DelistGameCommand(gameId);

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.Error.Should().Be(GameErrors.NotFound(gameId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenGameAlreadyDelisted()
    {
        // Arrange
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());
        Guid gameId = await Sender.AddGameAsync(categoryId);

        var command = new DelistGameCommand(gameId);

        await Sender.Send(command);

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.Error.Should().Be(GameErrors.AlreadyDelisted);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenGameIsDelisted()
    {
        // Arrange
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());
        Guid gameId = await Sender.AddGameAsync(categoryId);

        var command = new DelistGameCommand(gameId);

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
