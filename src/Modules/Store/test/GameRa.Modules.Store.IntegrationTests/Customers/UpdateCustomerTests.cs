using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Application.Customers.UpdateCustomer;
using GameRa.Modules.Store.Domain.Customers;
using GameRa.Modules.Store.IntegrationTests.Abstractions;

namespace GameRa.Modules.Store.IntegrationTests.Customers;

public class UpdateCustomerTests : BaseIntegrationTest
{
    public UpdateCustomerTests(IntegrationTestWebAppFactory factory) 
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        //Arrange
        var command = new UpdateCustomerCommand(
            Guid.NewGuid(),
            Faker.Internet.UserName());

        //Act
        Result result = await Sender.Send(command);

        //Assert
        result.Error.Should().Be(CustomerErrors.NotFound(command.CustomerId));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenCustomerIsUpdated()
    {
        //Arrange
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());

        var command = new UpdateCustomerCommand(
            customerId,
            Faker.Internet.UserName());

        //Act
        Result result = await Sender.Send(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
    }
}