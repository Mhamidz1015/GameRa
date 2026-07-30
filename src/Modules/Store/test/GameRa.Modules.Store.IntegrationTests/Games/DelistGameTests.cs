using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Games.DelistGame;
using GameRa.Modules.Games.Domain.Games;
using GameRa.Modules.Store.IntegrationTests.Abstractions;

namespace GameRa.Modules.Store.IntegrationTests.Games;

public class DelistGameTests : BaseIntegrationTest
{
    private const decimal Quantity = 10;

    public DelistGameTests(IntegrationTestWebAppFactory factory) 
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenGameDoesNotExist()
    {
        //Arrange
        var gameId = Guid.NewGuid();

        var command = new DelistGameCommand(gameId);

        //Act
        Result result = await Sender.Send(command);

        //Assert
        result.Error.Should().Be(GameErrors.NotFound(command.GameId));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenGameIsDelisted()
    {
        //Arrange
        var gameId = Guid.NewGuid();

        await Sender.AddGameAsync(gameId);

        var command = new DelistGameCommand(gameId);

        //Act
        Result result = await Sender.Send(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
    }
}
