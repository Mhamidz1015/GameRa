using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Carts.RemoveItemFromCart;
using GameRa.Modules.Store.Domain.Customers;
using GameRa.Modules.Store.IntegrationTests.Abstractions;

namespace GameRa.Modules.Store.IntegrationTests.Carts;

public class RemoveItemFromCartTests : BaseIntegrationTest
{
    private const decimal Quantity = 10;

    public RemoveItemFromCartTests(IntegrationTestWebAppFactory factory) :base(factory) {}

    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        //Arrange
        var command = new RemoveItemFromCartCommand(
            Guid.NewGuid(),
            Guid.NewGuid());

        //Act
        Result result = await Sender.Send(command);

        //Assert
        result.Error.Should().Be(CustomerErrors.NotFound(command.CustomerId));
    }


    [Fact]
    public async Task Should_ReturnSuccess_WhenRemovedItemFromCart()
    {
        //Arrange
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());
        var gameId = Guid.NewGuid();

        await Sender.AddGameAsync(gameId);

        var command = new RemoveItemFromCartCommand(
            customerId,
            gameId);

        //Act
        Result result = await Sender.Send(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
    }
}
