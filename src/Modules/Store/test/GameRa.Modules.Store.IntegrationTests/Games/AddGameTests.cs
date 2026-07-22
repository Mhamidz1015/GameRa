using Bogus;
using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Games.AddGame;
using GameRa.Modules.Store.IntegrationTests.Abstractions;
using MediatR;

namespace GameRa.Modules.Store.IntegrationTests.Games;

public class AddGameTests : BaseIntegrationTest
{
    public AddGameTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenGameIsCreated()
    {
        //Arrange
        var gameId = Guid.NewGuid();
     
        var command = new AddGameCommand(
            gameId,
            Faker.Commerce.ProductName(),
            Faker.Lorem.Sentence(),
            Faker.Company.CompanyName(),
            Faker.Random.Decimal(1, 200),
            DateTime.UtcNow.AddMonths(-1),
            Faker.Internet.Url());

    //Act
    Result result = await Sender.Send(command);

    //Assert
    result.IsSuccess.Should().BeTrue();

    }
}
