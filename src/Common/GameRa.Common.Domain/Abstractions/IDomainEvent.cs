using MediatR;

namespace GameRa.Common.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    Guid Id { get; }

    DateTime OccurredOnUtc { get; }
}
