using GameRa.Common.Application.MessagingGameBus;

namespace GameRa.Modules.Store.integrationEvents;

public sealed class OrderCompletedIntegrationEvent : IntegrationEvent
{
    public OrderCompletedIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid customerId,
        Guid gameId,
        string code)
        : base(id, occurredOnUtc)
    {
        CustomerId = customerId;
        GameId = gameId;
        Code = code;
    }

    public Guid CustomerId { get; init; }

    public Guid GameId { get; init; }

    public string Code { get; init; }
}
