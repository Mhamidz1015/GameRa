using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Domain.Customers;
using GameRa.Modules.Store.Domain.Games;
using GameRa.Modules.Store.Domain.Orders;
using GameRa.Modules.Store.UnitTests.Abstractions;

namespace GameRa.Modules.Store.UnitTests.Orders;

public class OrderTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenOrderIsCreated()
    {
        //Arrange
        Customer customer = CreateDefaultCustomer();

        //Act
        Result<Order> Orderesult = Order.Create(customer);

        //Assert
        OrderCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<OrderCreatedDomainEvent>(Orderesult.Value);

        domainEvent.OrderId.Should().Be(Orderesult.Value.Id);
    }
    [Fact]
    public void CompleteOrder_ShouldReturnFailure_WhenOrderAlreadyCompleted()
    {
        // Arrange
        Customer customer = CreateDefaultCustomer();

        Result<Order> result = Order.Create(customer);

        result.Value.CompleteOrder();
        Order order = result.Value;

        // Act
        Result orderesult = order.CompleteOrder();

        // Assert
        orderesult.Error.Should().Be(OrderErrors.OrderHasIssues);
    }

    [Fact]
    public void CompleteOrder_ShouldRaiseDomainEvent_WhenOrderIsPending()
    {
        // Arrange
        Customer customer = CreateDefaultCustomer();

        Result<Order> result = Order.Create(customer);


        // Act
        result.Value.CompleteOrder();

        // Assert
        OrderCompletedDomainEvent domainEvent =
            AssertDomainEventWasPublished<OrderCompletedDomainEvent>(result.Value);
        domainEvent.OrderId.Should().Be(result.Value.Id);
    }
    private Customer CreateDefaultCustomer() =>
        Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Internet.UserName()
        );

    [Fact]
    public void Refund_ShouldReturnFailure_WhenOrderIsNotCompleted()
    {
        // Arrange
        Order order = Order.Create(CreateDefaultCustomer());

        // Act
        Result result = order.Refund();

        // Assert
        result.Error.Should().Be(OrderErrors.NotCompleted);
    }

    [Fact]
    public void Refund_ShouldReturnFailure_WhenOrderAlreadyRefunded()
    {
        // Arrange
        Order order = Order.Create(CreateDefaultCustomer());
        order.CompleteOrder();
        order.Refund();

        // Act
        Result result = order.Refund();

        // Assert
        result.Error.Should().Be(OrderErrors.AlreadyRefunded);
    }

    [Fact]
    public void Refund_ShouldRaiseDomainEvent_WhenOrderIsCompleted()
    {
        // Arrange
        Order order = Order.Create(CreateDefaultCustomer());
        order.CompleteOrder();

        // Act
        Result result = order.Refund();

        // Assert
        result.IsSuccess.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Refunded);

        OrderRefundedDomainEvent domainEvent =
            AssertDomainEventWasPublished<OrderRefundedDomainEvent>(order);

        domainEvent.OrderId.Should().Be(order.Id);
        domainEvent.CustomerId.Should().Be(order.CustomerId);
    }

}
