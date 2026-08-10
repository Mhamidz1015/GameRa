using Bogus;
using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Discounts.Application.Discounts.CreateGameDiscount;
using GameRa.Modules.Discounts.Application.Discounts.CreateGlobalDiscount;
using GameRa.Modules.Discounts.Domain;
using GameRa.Modules.Reviews.Application.Reviews.CreateReview;
using GameRa.Modules.Reviews.Application.Reviews.DeleteReview;
using GameRa.Modules.Store.Application.Carts.AddItemToCart;
using GameRa.Modules.Store.Application.Customers.CreateCustomer;
using GameRa.Modules.Store.Application.Games.AddGame;
using GameRa.Modules.Store.Application.Orders.CreateOrder;
using GameRa.Modules.Users.Application.Users.RegisterUser;
using MediatR;

namespace GameRa.IntegrationTests.Abstractions;

internal static class CommandHelpers
{
    internal static async Task<Guid> RegisterUserAsync(
        this ISender sender)
    {
        var faker = new Faker();

        Result<Guid> result = await sender.Send(new RegisterUserCommand(
            faker.Internet.Email(),
            faker.Internet.Password(12),
            faker.Internet.UserName(),
            DateTime.UtcNow));

        result.IsSuccess.Should().BeTrue();

        return result.Value;
    }

    internal static async Task CreateCustomerAsync(
        this ISender sender,
        Guid userId)
    {
        var faker = new Faker();

        Result result = await sender.Send(new CreateCustomerCommand(
            userId,
            faker.Internet.Email(),
            faker.Internet.UserName()));

        result.IsSuccess.Should().BeTrue();
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
            faker.Random.Decimal(10, 200),
            DateTime.UtcNow.AddDays(-faker.Random.Int(30, 365)),
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

    internal static async Task CreateOrderAsync(
        this ISender sender,
        Guid userId)
    {
        Result result = await sender.Send(new CreateOrderCommand(userId));

        result.IsSuccess.Should().BeTrue();
    }

    internal static async Task CreateReviewAsync(
        this ISender sender,
        Guid gameId,
        Guid userId,
        int rating,
        bool isVerifiedPurchase = false)
    {
        var faker = new Faker();

        Result<Guid> result = await sender.Send(new CreateReviewCommand(
            gameId,
            userId,
            rating,
            faker.Lorem.Sentence(),
            isVerifiedPurchase));

        result.IsSuccess.Should().BeTrue();
    }

    internal static async Task<Guid> CreateReviewAndGetIdAsync(
        this ISender sender,
        Guid gameId,
        Guid userId,
        int rating,
        bool isVerifiedPurchase = false)
    {
        var faker = new Faker();

        Result<Guid> result = await sender.Send(new CreateReviewCommand(
            gameId,
            userId,
            rating,
            faker.Lorem.Sentence(),
            isVerifiedPurchase));

        result.IsSuccess.Should().BeTrue();

        return result.Value;
    }

    internal static async Task DeleteReviewAsync(
        this ISender sender,
        Guid reviewId,
        Guid userId)
    {
        Result result = await sender.Send(new DeleteReviewCommand(reviewId, userId));

        result.IsSuccess.Should().BeTrue();
    }

    internal static async Task CreateGameDiscountAsync(
        this ISender sender,
        Guid gameId,
        DiscountType discountType,
        decimal amount,
        DateTime? startsAt = null,
        DateTime? endsAt = null)
    {
        var faker = new Faker();

        Result result = await sender.Send(new CreateGameDiscountCommand(
            $"GAME{faker.Random.AlphaNumeric(8).ToUpperInvariant()}",
            discountType,
            amount,
            gameId,
            startsAt ?? DateTime.UtcNow,
            endsAt ?? DateTime.UtcNow.AddDays(faker.Random.Int(7, 30))));

        result.IsSuccess.Should().BeTrue();
    }

    internal static async Task<string> CreateGlobalDiscountAsync(
        this ISender sender,
        DiscountType discountType,
        decimal amount,
        DateTime? startsAt = null,
        DateTime? endsAt = null)
    {
        var faker = new Faker();

        string code = $"GLOBAL{faker.Random.AlphaNumeric(8).ToUpperInvariant()}";

        Result result = await sender.Send(new CreateGlobalDiscountCommand(
            code,
            discountType,
            amount,
            startsAt ?? DateTime.UtcNow,
            endsAt ?? DateTime.UtcNow.AddDays(faker.Random.Int(7, 30))));

        result.IsSuccess.Should().BeTrue();

        return code;
    }
}
