using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.IntegrationTests.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.GetUserLibrary;
using GameRa.Modules.Store.Application.Customers.GetCustomer;
using GameRa.Modules.Users.Application.Users.RegisterUser;

namespace GameRa.IntegrationTests.RegisterUser;

public sealed class RegisterUserTests : BaseIntegrationTest
{
    public RegisterUserTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task RegisterUser_Should_PropagateToStoreModule()
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

        // Assert
        customerResult.IsSuccess.Should().BeTrue();
        customerResult.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task RegisterUser_Should_PropagateToLibraryModule()
    {
        // Register user
        var command = new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password(6),
            Faker.Internet.UserName(),
            DateTime.UtcNow);

        Result<Guid> userResult = await Sender.Send(command);

        userResult.IsSuccess.Should().BeTrue();

        // Get libraryItems
        Result<IReadOnlyCollection<LibraryItemResponse>> LibraryResult = await Poller.WaitAsync(
            TimeSpan.FromSeconds(15),
            async () =>
            {
                var query = new GetUserLibraryQuery(userResult.Value);

                Result<IReadOnlyCollection<LibraryItemResponse>> customerResult = await Sender.Send(query);

                return customerResult;
            });

        // Assert
        LibraryResult.IsSuccess.Should().BeTrue();
        LibraryResult.Value.Should().NotBeEmpty();
    }
}
