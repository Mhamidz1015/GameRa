using GameRa.Common.Application.Messaging;
using GameRa.Modules.Discounts.Domain;

namespace GameRa.Modules.Discounts.Application.Discounts.CreateCategoryDiscount;

public sealed record CreateCategoryDiscountCommand(
    string Code,
    DiscountType Type,
    decimal Amount,
    Guid CategoryId,
    DateTime StartDateTimeUtc,
    DateTime EndDateTimeUtc) : ICommand<Guid>;