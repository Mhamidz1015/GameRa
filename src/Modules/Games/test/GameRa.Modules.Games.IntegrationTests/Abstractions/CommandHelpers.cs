using Bogus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Application.Categories.CreateCategory;
using GameRa.Modules.Games.Application.Games.AddGame;
using MediatR;

namespace GameRa.Modules.Games.IntegrationTests.Abstractions;

internal static class CommandHelpers
{
    internal static async Task<Guid> CreateCategoryAsync(this ISender sender, string name)
    {
        Result<Guid> result = await sender.Send(new CreateCategoryCommand(name));

        return result.Value;
    }

    internal static async Task<Guid> AddGameAsync(
        this ISender sender,
        Guid categoryId)
    {
        var faker = new Faker();
        Result<Guid> result = await sender.Send(
            new AddGameCommand(
                categoryId,
                faker.Commerce.ProductName(),
                faker.Lorem.Sentence(),
                faker.Company.CompanyName(),
                DateTime.UtcNow.AddMonths(-1),
                faker.Random.Decimal(1, 200),
                faker.Internet.Url()));

        return result.Value;
    }
}
