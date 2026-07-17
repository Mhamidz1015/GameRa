using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Domain.Customers;
using GameRa.Modules.Store.UnitTests.Abstractions;

namespace GameRa.Modules.Store.UnitTests.Customers;

public class CustomerTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnValue_WhenCustomerIsCreated()
    {
        //Act
        Result<Customer> result = Customer.Create(
            Guid.NewGuid(), 
            Faker.Internet.Email(),
            Faker.Internet.UserName());
        //Assert
        result.Value.Should().NotBeNull();
    }
}
