using GameRa.Common.Domain.Abstractions;
using MediatR;

namespace GameRa.Common.Application.Messaging;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;
