namespace GameRa.Modules.Discounts.PublicApi;

public interface IDiscountsApi
{
    Task<DiscountApiResponse?> GetActiveDiscountForGameAsync(
        Guid gameId,
        Guid categoryId,
        string? couponCode,
        CancellationToken cancellationToken = default);
}
