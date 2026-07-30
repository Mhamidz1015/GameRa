using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.IntegrationTests.Abstractions;
using GameRa.Modules.Games.Domain.Categories;
using GameRa.Modules.Store.Application.Carts.AddItemToCart;
using GameRa.Modules.Store.Application.Customers.GetCustomer;
using GameRa.Modules.Users.Application.Users.RegisterUser;

namespace GameRa.IntegrationTests.AddToCart;

public sealed class AddItemToCartTests : BaseIntegrationTest
{
    public AddItemToCartTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Customer_ShouldBeAbleTo_AddItemToCart()
    {
        // Register user
        var command = new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password(6),
            Faker.Internet.UserName(),
            DateTime.UtcNow);

        Result<Guid> userResult = await Sender.Send(command);

        userResult.IsSuccess.Should().BeTrue();

        // Get customer
        Result<CustomerResponse> customerResult = await Poller.WaitAsync(
            TimeSpan.FromSeconds(15),
            async () =>
            {
                var query = new GetCustomerQuery(userResult.Value);

                Result<CustomerResponse> customerResult = await Sender.Send(query);

                return customerResult;
            });

        customerResult.IsSuccess.Should().BeTrue();

        // Add item to cart
        CustomerResponse customer = customerResult.Value;
        var gameId = Guid.NewGuid();

        await Sender.AddGameAsync(Guid.NewGuid());

        Result result = await Sender.Send(new AddItemToCartCommand(customer.Id, gameId));

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
