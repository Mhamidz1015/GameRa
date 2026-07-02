namespace GameRa.Common.Application.MessagingGameBus;

public interface IGameBus
{
    Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
        where T : IIntegrationEvent;
}
