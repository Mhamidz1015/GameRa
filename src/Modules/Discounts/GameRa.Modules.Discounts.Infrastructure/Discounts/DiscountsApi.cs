using GameRa.Modules.Discounts.Domain;
using GameRa.Modules.Discounts.Infrastructure.Database;
using GameRa.Modules.Discounts.PublicApi;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Discounts.Infrastructure.Discounts;

internal sealed class DiscountsApi(DiscountDbContext context) : IDiscountsApi
{
    public async Task<DiscountApiResponse?> GetActiveDiscountForGameAsync(
        Guid gameId,
        Guid categoryId,
        string? couponCode,
        CancellationToken cancellationToken = default)
    {
        DateTime utcNow = DateTime.UtcNow;

        IQueryable<Discount> query = context.Discounts
            .Where(d => d.IsActive &&
                        d.StartDateTimeUtc <= utcNow &&
                        d.EndDateTimeUtc >= utcNow);

        if (!string.IsNullOrWhiteSpace(couponCode))
        {
            query = query.Where(d => d.Code == couponCode.ToUpperInvariant());
        }
        else
        {
            query = query.Where(d =>
                (d.Scope == DiscountScope.Game && d.GameId == gameId) ||
                (d.Scope == DiscountScope.Category && d.CategoryId == categoryId) ||
                d.Scope == DiscountScope.Global);
        }

        Discount? discount = await query
            .OrderByDescending(d => d.Amount)
            .FirstOrDefaultAsync(cancellationToken);

        if (discount is null)
        {
            return null;
        }

        return new DiscountApiResponse(
            discount.DiscountId,
            discount.Code,
            discount.Amount,
            discount.Type == DiscountType.Percentage);
    }
}
