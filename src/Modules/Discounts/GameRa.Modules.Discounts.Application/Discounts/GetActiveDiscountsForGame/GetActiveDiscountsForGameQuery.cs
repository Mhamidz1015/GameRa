using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Discounts.Application.Discounts.GetActiveDiscountsForGame;

public sealed record GetActiveDiscountsForGameQuery(Guid GameId, Guid CategoryId)
    : IQuery<IReadOnlyCollection<GetDiscount.DiscountResponse>>;