namespace GameRa.Modules.Discounts.PublicApi;

public sealed record DiscountApiResponse(
    Guid DiscountId,
    string Code,
    decimal Amount,
    bool IsPercentage);
