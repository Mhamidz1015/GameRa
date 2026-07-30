using Bogus;
using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Application.Games.AddGame;
using GameRa.Modules.Games.Domain.Categories;
using MediatR;

namespace GameRa.IntegrationTests.Abstractions;

internal static class CommandHelpers
{
    internal static async Task AddGameAsync(
        this ISender sender,
        Guid categoryId)
    {
        var faker = new Faker();

        Result result = await sender.Send(new AddGameCommand(
                categoryId,
                faker.Commerce.ProductName(),
                faker.Lorem.Sentence(),
                faker.Company.CompanyName(),
                DateTime.UtcNow.AddMonths(-1),
                faker.Random.Decimal(1, 200),
                faker.Internet.Url()));

        result.IsSuccess.Should().BeTrue();
    }
}
