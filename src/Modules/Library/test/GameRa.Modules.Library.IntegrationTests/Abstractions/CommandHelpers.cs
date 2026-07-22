using Bogus;
using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.AddGameToLibrary;
using GameRa.Modules.Library.Application.LibraryItems.ArchivedLibraryItem;
using MediatR;

namespace GameRa.Modules.Library.IntegrationTests.Abstractions;

internal static class CommandHelpers
{
    internal static async Task AddGameToLibraryAsync(
        this ISender sender,
        Guid userId,
        Guid gameId)
    {
        var faker = new Faker();
        Result result = await sender.Send(new AddGameToLibraryCommand(
            userId,
            gameId,
            faker.Commerce.ProductName()));

        result.IsSuccess.Should().BeTrue();
    }

    internal static async Task ArchiveLibraryItemAsync(
        this ISender sender,
        Guid userId,
        Guid gameId)
    {
        Result result = await sender.Send(new ArchivedLibraryItemCommand(
            userId,
            gameId));

        result.IsSuccess.Should().BeTrue();
    }
}
