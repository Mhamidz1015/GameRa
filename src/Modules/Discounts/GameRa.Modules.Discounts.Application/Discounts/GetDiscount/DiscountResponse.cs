namespace GameRa.Modules.Discounts.Application.Discounts.GetDiscount;

public sealed record DiscountResponse(
    Guid DiscountId,
    string Code,
    int Type,
    decimal Amount,
    int Scope,
    Guid? GameId,
    Guid? CategoryId,
    DateTime StartDateTimeUtc,
    DateTime EndDateTimeUtc,
    bool IsActive);