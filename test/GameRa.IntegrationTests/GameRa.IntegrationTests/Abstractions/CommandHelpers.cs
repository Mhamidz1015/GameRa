using Bogus;
using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Games.AddGame;
using MediatR;

namespace GameRa.IntegrationTests.Abstractions;

internal static class CommandHelpers
{
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
