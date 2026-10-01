using Bogus;
using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Carts.AddItemToCart;
using GameRa.Modules.Store.Application.Customers.CreateCustomer;
using GameRa.Modules.Store.Application.Games.AddGame;
using GameRa.Modules.Store.Application.Orders.CreateOrder;
using GameRa.Modules.Store.Application.Orders.GetOrdersByCustomerId;
using MediatR;

namespace GameRa.Modules.Store.IntegrationTests.Abstractions;

internal static class CommandHelpers
{
    internal static async Task<Guid> CreateCustomerAsync(this ISender sender, Guid customerId)
    {
        var faker = new Faker();
        Result result = await sender.Send(
            new CreateCustomerCommand(
                customerId,
                faker.Internet.Email(),
                faker.Internet.UserName()));

        result.IsSuccess.Should().BeTrue();

        return customerId;
    }

    internal static async Task AddGameAsync(
    this ISender sender,
    Guid gameId)
    {
        var faker = new Faker();
        Result result = await sender.Send(new AddGameCommand(
            gameId,
            faker.Commerce.ProductName(),
            faker.Lorem.Sentence(),
            faker.Company.CompanyName(),
            faker.Random.Decimal(1, 200),
            DateTime.UtcNow.AddMonths(-1),
            faker.Internet.Url()));

        result.IsSuccess.Should().BeTrue();
    }

    internal static async Task AddItemToCartAsync(
        this ISender sender,
        Guid userId,
        Guid gameId)
    {
        Result result = await sender.Send(new AddItemToCartCommand(userId, gameId));

        result.IsSuccess.Should().BeTrue();
    }

    internal static async Task<Guid> CreateOrderAsync(this ISender sender, Guid customerId)
    {
        Result result = await sender.Send(new CreateOrderCommand(customerId));
        result.IsSuccess.Should().BeTrue();

        Result<IReadOnlyCollection<OrderSummaryResponse>> orders =
            await sender.Send(new GetOrdersByCustomerIdQuery(customerId));
        orders.IsSuccess.Should().BeTrue();

        return orders.Value.First().Id;
    }
}
