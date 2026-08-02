using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.IntegrationTests.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.GetUserLibrary;
using GameRa.Modules.Store.Application.Carts.AddItemToCart;
using GameRa.Modules.Store.Application.Customers.CreateCustomer;
using GameRa.Modules.Store.Application.Games.AddGame;
using GameRa.Modules.Store.Application.Orders.CreateOrder;
using GameRa.Modules.Users.Application.Users.RegisterUser;

namespace GameRa.IntegrationTests.Orders;

public sealed class OrderTests : BaseIntegrationTest
{
    public OrderTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Order_Should_AddGameToLibrary()
    {
        // Register user
        var registerCommand = new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password(6),
            Faker.Internet.UserName(),
            DateTime.UtcNow);

        Result<Guid> userResult = await Sender.Send(registerCommand);
        userResult.IsSuccess.Should().BeTrue();

        Guid userId = userResult.Value;

        // Create customer in Store module
        Result createCustomerResult = await Sender.Send(new CreateCustomerCommand(
            userId,
            Faker.Internet.Email(),
            Faker.Internet.UserName()));
        createCustomerResult.IsSuccess.Should().BeTrue();

        // Add game to Store
        Guid gameId = Guid.NewGuid();
        Result addGameResult = await Sender.Send(new AddGameCommand(
            gameId,
            Faker.Commerce.ProductName(),
            Faker.Lorem.Sentence(),
            Faker.Company.CompanyName(),
            Faker.Random.Decimal(1, 200),
            DateTime.UtcNow.AddMonths(-1),
            Faker.Internet.Url()));
        addGameResult.IsSuccess.Should().BeTrue();

        // Add item to cart
        Result addToCartResult = await Sender.Send(
            new AddItemToCartCommand(userId, gameId));
        addToCartResult.IsSuccess.Should().BeTrue();

        // Create order
        Result orderResult = await Sender.Send(new CreateOrderCommand(userId));
        orderResult.IsSuccess.Should().BeTrue();

        // Wait for game to appear in Library
        Result<IReadOnlyCollection<LibraryItemResponse>> libraryResult = await Poller.WaitAsync(
            TimeSpan.FromSeconds(30),
            async () =>
            {
                var query = new GetUserLibraryQuery(userId);
                Result<IReadOnlyCollection<LibraryItemResponse>> result = await Sender.Send(query);

                if (result.IsFailure || !result.Value.Any(x => x.GameId == gameId))
                    return Result.Failure<IReadOnlyCollection<LibraryItemResponse>>(
                        Error.Failure("Library.Empty", "Game not in library yet"));

                return result;
            });

        // Assert
        libraryResult.IsSuccess.Should().BeTrue();
        libraryResult.Value.Should().Contain(x => x.GameId == gameId);
    }
}