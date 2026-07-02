namespace GameRa.Common.Application.MessagingGameBus;

public interface IIntegrationEvent
{
    Guid Id { get; }

    DateTime OccurredOnUtc { get; }
}
