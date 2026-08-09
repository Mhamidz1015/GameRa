using GameRa.Common.Application.Messaging;
using GameRa.Modules.Discounts.Domain;

namespace GameRa.Modules.Discounts.Application.Discounts.CreateGlobalDiscount;

public sealed record CreateGlobalDiscountCommand(
    string Code,
    DiscountType Type,
    decimal Amount,
    DateTime StartDateTimeUtc,
    DateTime EndDateTimeUtc) : ICommand<Guid>;