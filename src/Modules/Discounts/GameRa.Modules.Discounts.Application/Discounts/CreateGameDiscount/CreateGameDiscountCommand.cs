using GameRa.Common.Application.Messaging;
using GameRa.Modules.Discounts.Domain;

namespace GameRa.Modules.Discounts.Application.Discounts.CreateGameDiscount;

public sealed record CreateGameDiscountCommand(
    string Code,
    DiscountType Type,
    decimal Amount,
    Guid GameId,
    DateTime StartDateTimeUtc,
    DateTime EndDateTimeUtc) : ICommand<Guid>;