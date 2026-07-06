using GameRa.Common.Application.Messaging;
using GameRa.Modules.Store.Application.Abstractions.Payments;
using GameRa.Modules.Store.Domain.Payments;

namespace GameRa.Modules.Store.Application.Payments.RefundPayment;

internal sealed class PaymentRefundedDomainEventHandler(IPaymentService paymentService)
    : DomainEventHandler<PaymentRefundedDomainEvent>
{
    public override async Task Handle(
        PaymentRefundedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await paymentService.RefundAsync(domainEvent.TransactionId, domainEvent.RefundAmount);
    }
}
