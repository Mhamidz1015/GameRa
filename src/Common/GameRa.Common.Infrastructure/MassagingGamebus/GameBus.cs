using GameRa.Common.Application.MessagingGameBus;
using MassTransit;

namespace GameRa.Common.Infrastructure.MassagingGamebus;

internal sealed class GameBus(IBus bus) : IGameBus
{
    public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
        where T : IIntegrationEvent
    {
        await bus.Publish(integrationEvent, cancellationToken);
    }
}
