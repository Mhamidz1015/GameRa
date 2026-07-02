using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Store.Domain.Payments;

public sealed class PaymentCreatedDomainEvent(Guid paymentId) : DomainEvent
{
    public Guid PaymentId { get; init; } = paymentId;
}
