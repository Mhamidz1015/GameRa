using GameRa.Common.Application.MessagingEventBus;
using MassTransit;

namespace GameRa.Common.Infrastructure.MassagingEventbus;

internal sealed class EventBus(IBus bus) : IEventBus
{
    public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
        where T : IIntegrationEvent
    {
        await bus.Publish(integrationEvent, cancellationToken);
    }
}
