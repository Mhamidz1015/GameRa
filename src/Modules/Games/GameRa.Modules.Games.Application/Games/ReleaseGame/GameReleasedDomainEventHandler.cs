using GameRa.Common.Application.Messaging;
using GameRa.Modules.Games.Domain.Games;

namespace GameRa.Modules.Games.Application.Games.ReleaseGame;

internal sealed class GameReleasedDomainEventHandler : DomainEventHandler<GameReleasedDomainEvent>
{
    public override Task Handle(GameReleasedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
