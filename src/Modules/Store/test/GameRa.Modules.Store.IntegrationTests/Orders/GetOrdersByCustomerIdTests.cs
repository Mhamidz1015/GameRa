using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Orders.GetOrdersByCustomerId;
using GameRa.Modules.Store.IntegrationTests.Abstractions;

namespace GameRa.Modules.Store.IntegrationTests.Orders;

public sealed class GetOrdersByCustomerIdTests : BaseIntegrationTest
{
    public GetOrdersByCustomerIdTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task GetOrdersByCustomerId_ShouldReturnAllOrders()
    {
        Guid customerId = Faker.Random.Guid();

        await Sender.CreateCustomerAsync(customerId);
        Guid gameId = Faker.Random.Guid();
        await Sender.AddGameAsync(gameId);
        await Sender.AddItemToCartAsync(customerId, gameId);
        await Sender.CreateOrderAsync(customerId);

        Result<IReadOnlyCollection<OrderSummaryResponse>> result =
            await Sender.Send(new GetOrdersByCustomerIdQuery(customerId));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCountGreaterThanOrEqualTo(1);
        result.Value.Should().AllSatisfy(o => o.CustomerId.Should().Be(customerId));
    }

    [Fact]
    public async Task GetOrdersByCustomerId_ShouldReturnEmpty_WhenNoOrders()
    {
        Result<IReadOnlyCollection<OrderSummaryResponse>> result =
            await Sender.Send(new GetOrdersByCustomerIdQuery(Faker.Random.Guid()));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task GetOrdersByCustomerId_ShouldNotReturn_OtherCustomersOrders()
    {
        Guid customerId = Faker.Random.Guid();
        Guid otherCustomerId = Faker.Random.Guid();

        await Sender.CreateCustomerAsync(customerId);
        await Sender.CreateCustomerAsync(otherCustomerId);

        Guid gameId1 = Faker.Random.Guid();
        Guid gameId2 = Faker.Random.Guid();

        await Sender.AddGameAsync(gameId1);
        await Sender.AddGameAsync(gameId2);

        await Sender.AddItemToCartAsync(customerId, gameId1);
        await Sender.CreateOrderAsync(customerId);

        await Sender.AddItemToCartAsync(otherCustomerId, gameId2);
        await Sender.CreateOrderAsync(otherCustomerId);

        Result<IReadOnlyCollection<OrderSummaryResponse>> result =
            await Sender.Send(new GetOrdersByCustomerIdQuery(customerId));

        result.Value.Should().AllSatisfy(o => o.CustomerId.Should().Be(customerId));
    }
}