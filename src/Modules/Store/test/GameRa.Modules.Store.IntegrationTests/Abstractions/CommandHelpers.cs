using Bogus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Customers.CreateCustomer;
using MediatR;
using FluentAssertions;
using GameRa.Modules.Store.Application.Games.AddGame;

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
}
