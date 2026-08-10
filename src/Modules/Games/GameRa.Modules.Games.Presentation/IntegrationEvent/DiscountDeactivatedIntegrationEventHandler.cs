using GameRa.Common.Application.MessagingEventBus;
using GameRa.Modules.Discounts.IntegrationEvents;
using GameRa.Modules.Games.Application.Abstractions.Data;
using GameRa.Modules.Games.Domain.Games;

namespace GameRa.Modules.Games.Presentation.IntegrationEvent;

internal sealed class DiscountDeactivatedIntegrationEventHandler(
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork)
    : IntegrationEventHandler<DiscountDeactivatedIntegrationEvent>
{
    public override async Task Handle(
        DiscountDeactivatedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        if (integrationEvent.Scope == 3 && integrationEvent.GameId.HasValue)
        {
            Game? game = await gameRepository.GetAsync(integrationEvent.GameId.Value, cancellationToken);
            if (game is null) return;

            game.RemoveDiscount();
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
