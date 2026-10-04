using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.GetFavorites;
using GameRa.Modules.Library.Application.LibraryItems.GetUserLibrary;
using GameRa.Modules.Library.Application.LibraryItems.ToggleFavorite;
using GameRa.Modules.Library.Domain.LibraryItems;
using GameRa.Modules.Library.IntegrationTests.Abstractions;

namespace GameRa.Modules.Library.IntegrationTests.LibraryItems;

public sealed class FavoriteTests : BaseIntegrationTest
{
    public FavoriteTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task ToggleFavorite_ShouldSetFavorite_WhenNotFavorited()
    {
        Guid userId = Faker.Random.Guid();
        Guid gameId = Faker.Random.Guid();

        await Sender.AddGameToLibraryAsync(userId, gameId);

        Result result = await Sender.Send(new ToggleFavoriteCommand(userId, gameId));

        result.IsSuccess.Should().BeTrue();

        Result<IReadOnlyCollection<LibraryItemResponse>> favorites =
            await Sender.Send(new GetFavoritesQuery(userId));

        favorites.Value.Should().Contain(f => f.GameId == gameId);
    }

    [Fact]
    public async Task ToggleFavorite_ShouldUnsetFavorite_WhenAlreadyFavorited()
    {
        Guid userId = Faker.Random.Guid();
        Guid gameId = Faker.Random.Guid();

        await Sender.AddGameToLibraryAsync(userId, gameId);

        await Sender.Send(new ToggleFavoriteCommand(userId, gameId));
        await Sender.Send(new ToggleFavoriteCommand(userId, gameId));

        Result<IReadOnlyCollection<LibraryItemResponse>> favorites =
            await Sender.Send(new GetFavoritesQuery(userId));

        favorites.Value.Should().NotContain(f => f.GameId == gameId);
    }

    [Fact]
    public async Task ToggleFavorite_ShouldFail_WhenGameNotInLibrary()
    {
        Guid gameId = Faker.Random.Guid();

        Result result = await Sender.Send(
            new ToggleFavoriteCommand(Faker.Random.Guid(), gameId));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(LibraryItemErrors.NotOwned(gameId));
    }

    [Fact]
    public async Task GetFavorites_ShouldReturnOnlyFavorites()
    {
        Guid userId = Faker.Random.Guid();
        Guid favGameId = Faker.Random.Guid();
        Guid notFavGameId = Faker.Random.Guid();

        await Sender.AddGameToLibraryAsync(userId, favGameId);
        await Sender.AddGameToLibraryAsync(userId, notFavGameId);

        await Sender.Send(new ToggleFavoriteCommand(userId, favGameId));

        Result<IReadOnlyCollection<LibraryItemResponse>> favorites =
            await Sender.Send(new GetFavoritesQuery(userId));

        favorites.IsSuccess.Should().BeTrue();
        favorites.Value.Should().ContainSingle();
        favorites.Value.Should().Contain(f => f.GameId == favGameId);
        favorites.Value.Should().NotContain(f => f.GameId == notFavGameId);
    }

    [Fact]
    public async Task GetFavorites_ShouldReturnEmpty_WhenNoFavorites()
    {
        Guid userId = Faker.Random.Guid();
        Guid gameId = Faker.Random.Guid();

        await Sender.AddGameToLibraryAsync(userId, gameId);

        Result<IReadOnlyCollection<LibraryItemResponse>> favorites =
            await Sender.Send(new GetFavoritesQuery(userId));

        favorites.IsSuccess.Should().BeTrue();
        favorites.Value.Should().BeEmpty();
    }
}