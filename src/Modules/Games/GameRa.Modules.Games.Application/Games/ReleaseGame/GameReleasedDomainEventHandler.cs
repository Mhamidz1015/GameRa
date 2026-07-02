using GameRa.Common.Application.Messaging;
using GameRa.Modules.Games.Domain.Games;

namespace GameRa.Modules.Games.Application.Games.ReleaseGame;

internal sealed class GameReleasedDomainEventHandler : IDomainEventHandler<GameReleasedDomainEvent>
{
    public Task Handle(GameReleasedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
