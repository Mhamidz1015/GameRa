using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Domain.Customers;
using GameRa.Modules.Store.Domain.Orders;
using GameRa.Modules.Store.Domain.Payments;
using GameRa.Modules.Store.UnitTests.Abstractions;

namespace GameRa.Modules.Store.UnitTests.Payments;

public class PaymentTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenPaymentIsCreated()
    {
        //Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Internet.UserName());

        var order = Order.Create(customer);

        //Act
        Result<Payment> result = Payment.Create(
            order,
            Guid.NewGuid(),
            Faker.Random.Decimal());

        //Assert
        PaymentCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<PaymentCreatedDomainEvent>(result.Value);

        domainEvent.PaymentId.Should().Be(result.Value.Id);
    }

    [Fact]
    public void Refund_ShouldReturnFailure_WhenAlreadyRefunded()
    {
        //Arrange
        decimal amount = Faker.Random.Decimal();

        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Internet.UserName());

        var order = Order.Create(customer);

        Result<Payment> paymentResult = Payment.Create(
            order,
            Guid.NewGuid(),
            amount);

        Payment payment = paymentResult.Value;

        payment.Refund(amount);

        //Act
        Result result = payment.Refund(amount);

        //Assert
        result.Error.Should().Be(PaymentErrors.AlreadyRefunded);
    }
}
