using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Orders.GetOrder;
using GameRa.Modules.Store.Application.Orders.RefundOrder;
using GameRa.Modules.Store.Domain.Orders;
using GameRa.Modules.Store.IntegrationTests.Abstractions;

namespace GameRa.Modules.Store.IntegrationTests.Orders;

public sealed class RefundOrderTests : BaseIntegrationTest
{
    public RefundOrderTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task RefundOrder_ShouldSucceed_WhenOrderIsCompleted()
    {
        Guid customerId = Faker.Random.Guid();
        await Sender.CreateCustomerAsync(customerId);

        Guid gameId = Faker.Random.Guid();
        await Sender.AddGameAsync(gameId);
        await Sender.AddItemToCartAsync(customerId, gameId);
        Guid orderId = await Sender.CreateOrderAsync(customerId);

        Result result = await Sender.Send(new RefundOrderCommand(orderId, customerId));

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task RefundOrder_ShouldFail_WhenAlreadyRefunded()
    {
        Guid customerId = Faker.Random.Guid();
        await Sender.CreateCustomerAsync(customerId);

        Guid gameId = Faker.Random.Guid();
        await Sender.AddGameAsync(gameId);
        await Sender.AddItemToCartAsync(customerId, gameId);
        Guid orderId = await Sender.CreateOrderAsync(customerId);

        await Sender.Send(new RefundOrderCommand(orderId, customerId));
        Result result = await Sender.Send(new RefundOrderCommand(orderId, customerId));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.AlreadyRefunded);
    }

    [Fact]
    public async Task RefundOrder_ShouldFail_WhenUserIsNotOwner()
    {
        Guid customerId = Faker.Random.Guid();
        await Sender.CreateCustomerAsync(customerId);

        Guid gameId = Faker.Random.Guid();
        await Sender.AddGameAsync(gameId);
        await Sender.AddItemToCartAsync(customerId, gameId);
        Guid orderId = await Sender.CreateOrderAsync(customerId);

        Result result = await Sender.Send(
            new RefundOrderCommand(orderId, Faker.Random.Guid()));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.Forbidden);
    }

    [Fact]
    public async Task RefundOrder_ShouldFail_WhenOrderDoesNotExist()
    {
        Guid orderId = Faker.Random.Guid();

        Result result = await Sender.Send(
            new RefundOrderCommand(orderId, Faker.Random.Guid()));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.NotFound(orderId));
    }

    [Fact]
    public async Task RefundOrder_ShouldSetStatusToRefunded()
    {
        Guid customerId = Faker.Random.Guid();
        await Sender.CreateCustomerAsync(customerId);

        Guid gameId = Faker.Random.Guid();
        await Sender.AddGameAsync(gameId);
        await Sender.AddItemToCartAsync(customerId, gameId);
        Guid orderId = await Sender.CreateOrderAsync(customerId);

        await Sender.Send(new RefundOrderCommand(orderId, customerId));

        Result<OrderResponse> order = await Sender.Send(new GetOrderQuery(orderId));

        order.Value.Status.Should().Be(OrderStatus.Refunded);
    }
}
