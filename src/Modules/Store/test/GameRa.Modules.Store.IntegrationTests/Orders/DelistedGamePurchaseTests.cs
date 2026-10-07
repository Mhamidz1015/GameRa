using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Carts.AddItemToCart;
using GameRa.Modules.Store.Application.Games.DelistGame;
using GameRa.Modules.Store.Application.Orders.CreateOrder;
using GameRa.Modules.Store.Domain.Games;
using GameRa.Modules.Store.IntegrationTests.Abstractions;

namespace GameRa.Modules.Store.IntegrationTests.Orders;

public class DelistedGamePurchaseTests : BaseIntegrationTest
{
    public DelistedGamePurchaseTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task AddItemToCart_ShouldFail_WhenGameIsDelisted()
    {
        //Arrange
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());
        Guid gameId = Guid.NewGuid();

        await Sender.AddGameAsync(gameId);
        await Sender.Send(new DelistGameCommand(gameId));

        //Act
        Result result = await Sender.Send(new AddItemToCartCommand(customerId, gameId));

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(GameErrors.Delisted(gameId));
    }

    [Fact]
    public async Task CreateOrder_ShouldFail_WhenGameWasDelistedAfterBeingAddedToCart()
    {
        //Arrange
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());
        Guid gameId = Guid.NewGuid();

        await Sender.AddGameAsync(gameId);
        await Sender.AddItemToCartAsync(customerId, gameId);
        await Sender.Send(new DelistGameCommand(gameId));

        //Act
        Result result = await Sender.Send(new CreateOrderCommand(customerId));

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(GameErrors.Delisted(gameId));
    }
}