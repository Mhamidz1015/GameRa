using GameRa.Common.Application.MessagingEventBus;
using GameRa.Modules.Discounts.IntegrationEvents;
using GameRa.Modules.Games.Application.Abstractions.Data;
using GameRa.Modules.Games.Domain.Games;

namespace GameRa.Modules.Games.Presentation.IntegrationEvents;

internal sealed class DiscountActivatedIntegrationEventHandler(
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork)
    : IntegrationEventHandler<DiscountActivatedIntegrationEvent>
{
    public override async Task Handle(
        DiscountActivatedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        if (integrationEvent.Scope == 3 && integrationEvent.GameId.HasValue)
        {
            Game? game = await gameRepository.GetAsync(integrationEvent.GameId.Value, cancellationToken);
            if (game is null) return;

            game.ApplyDiscount(integrationEvent.Amount, integrationEvent.Type == 1);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
