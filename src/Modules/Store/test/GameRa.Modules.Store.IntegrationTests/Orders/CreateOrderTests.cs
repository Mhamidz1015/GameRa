using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Carts;
using GameRa.Modules.Store.Application.Carts.AddItemToCart;
using GameRa.Modules.Store.Application.Orders.CreateOrder;
using GameRa.Modules.Store.Domain.Customers;
using GameRa.Modules.Store.IntegrationTests.Abstractions;

namespace GameRa.Modules.Store.IntegrationTests.Orders;

public class CreateOrderTests : BaseIntegrationTest
{
    public CreateOrderTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        //Arrange
        var command = new CreateOrderCommand(Guid.NewGuid());

        //Act
        Result result = await Sender.Send(command);

        //Assert
        result.Error.Should().Be(CustomerErrors.NotFound(command.CustomerId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCartIsEmpty()
    {
        //Arrange
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());

        var command = new CreateOrderCommand(customerId);

        //Act
        Result result = await Sender.Send(command);

        //Assert
        result.Error.Should().Be(CartErrors.Empty);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenCartHasItems()
    {
        // Arrange
        await CleanDatabaseAsync();

        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());
        Guid gameId = Guid.NewGuid();

        await Sender.AddGameAsync(gameId);

        Result addItemResult = await Sender.Send( new AddItemToCartCommand(customerId, gameId));
        addItemResult.IsSuccess.Should().BeTrue();

        var command = new CreateOrderCommand(customerId);

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
