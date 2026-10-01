using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Wishlist.AddToWishlist;
using GameRa.Modules.Store.Application.Wishlist.GetWishlist;
using GameRa.Modules.Store.Application.Wishlist.RemoveFromWishlist;
using GameRa.Modules.Store.Domain.Wishlist;
using GameRa.Modules.Store.IntegrationTests.Abstractions;

namespace GameRa.Modules.Store.IntegrationTests.Wishlists;

public sealed class WishlistTests : BaseIntegrationTest
{
    public WishlistTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task AddToWishlist_ShouldSucceed_WhenGameNotInWishlist()
    {
        Guid customerId = Faker.Random.Guid();
        Guid gameId = Faker.Random.Guid();

        Result result = await Sender.Send(new AddToWishlistCommand(customerId, gameId));

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task AddToWishlist_ShouldFail_WhenAlreadyInWishlist()
    {
        Guid customerId = Faker.Random.Guid();
        Guid gameId = Faker.Random.Guid();

        await Sender.Send(new AddToWishlistCommand(customerId, gameId));
        Result result = await Sender.Send(new AddToWishlistCommand(customerId, gameId));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(WishlistErrors.AlreadyInWishlist);
    }

    [Fact]
    public async Task RemoveFromWishlist_ShouldSucceed_WhenExists()
    {
        Guid customerId = Faker.Random.Guid();
        Guid gameId = Faker.Random.Guid();

        await Sender.Send(new AddToWishlistCommand(customerId, gameId));
        Result result = await Sender.Send(new RemoveFromWishlistCommand(customerId, gameId));

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task RemoveFromWishlist_ShouldFail_WhenNotExists()
    {
        Result result = await Sender.Send(
            new RemoveFromWishlistCommand(Faker.Random.Guid(), Faker.Random.Guid()));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(WishlistErrors.NotFound);
    }

    [Fact]
    public async Task GetWishlist_ShouldReturnAllItems()
    {
        Guid customerId = Faker.Random.Guid();
        Guid gameId1 = Faker.Random.Guid();
        Guid gameId2 = Faker.Random.Guid();

        await Sender.Send(new AddToWishlistCommand(customerId, gameId1));
        await Sender.Send(new AddToWishlistCommand(customerId, gameId2));

        Result<IReadOnlyCollection<WishlistItemResponse>> result =
            await Sender.Send(new GetWishlistQuery(customerId));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Should().Contain(w => w.GameId == gameId1);
        result.Value.Should().Contain(w => w.GameId == gameId2);
    }

    [Fact]
    public async Task GetWishlist_ShouldReturnEmpty_WhenNoItems()
    {
        Result<IReadOnlyCollection<WishlistItemResponse>> result =
            await Sender.Send(new GetWishlistQuery(Faker.Random.Guid()));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task GetWishlist_ShouldNotReturn_OtherCustomersItems()
    {
        Guid customerId = Faker.Random.Guid();
        Guid otherCustomerId = Faker.Random.Guid();

        await Sender.Send(new AddToWishlistCommand(customerId, Faker.Random.Guid()));
        await Sender.Send(new AddToWishlistCommand(otherCustomerId, Faker.Random.Guid()));

        Result<IReadOnlyCollection<WishlistItemResponse>> result =
            await Sender.Send(new GetWishlistQuery(customerId));

        result.Value.Should().HaveCount(1);
        result.Value.Should().AllSatisfy(w => w.CustomerId.Should().Be(customerId));
    }
}