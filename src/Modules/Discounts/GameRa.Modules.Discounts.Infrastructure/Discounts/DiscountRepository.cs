using GameRa.Modules.Discounts.Domain;
using GameRa.Modules.Discounts.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Discounts.Infrastructure.Discounts;

internal sealed class DiscountRepository(DiscountDbContext context) : IDiscountRepository
{
    public async Task<Discount?> GetAsync(Guid discountId, CancellationToken cancellationToken = default)
    {
        return await context.Discounts
            .SingleOrDefaultAsync(d => d.DiscountId == discountId, cancellationToken);
    }

    public async Task<Discount?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await context.Discounts
            .SingleOrDefaultAsync(d => d.Code == code.ToUpperInvariant(), cancellationToken);
    }

    public async Task<List<Discount>> GetActiveByGameIdAsync(
        Guid gameId,
        DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        return await context.Discounts
            .Where(d => d.IsActive &&
                        d.Scope == DiscountScope.Game &&
                        d.GameId == gameId &&
                        d.StartDateTimeUtc <= utcNow &&
                        d.EndDateTimeUtc >= utcNow)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Discount>> GetActiveByCategoryIdAsync(
        Guid categoryId,
        DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        return await context.Discounts
            .Where(d => d.IsActive &&
                        d.Scope == DiscountScope.Category &&
                        d.CategoryId == categoryId &&
                        d.StartDateTimeUtc <= utcNow &&
                        d.EndDateTimeUtc >= utcNow)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Discount>> GetActiveGlobalAsync(
        DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        return await context.Discounts
            .Where(d => d.IsActive &&
                        d.Scope == DiscountScope.Global &&
                        d.StartDateTimeUtc <= utcNow &&
                        d.EndDateTimeUtc >= utcNow)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await context.Discounts
            .AnyAsync(d => d.Code == code.ToUpperInvariant(), cancellationToken);
    }

    public void Insert(Discount discount)
    {
        context.Discounts.Add(discount);
    }
}
