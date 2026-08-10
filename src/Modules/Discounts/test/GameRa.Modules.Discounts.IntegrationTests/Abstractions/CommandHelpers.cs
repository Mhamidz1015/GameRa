using Bogus;
using GameRa.Modules.Discounts.Application.Discounts.ActivateDiscount;
using GameRa.Modules.Discounts.Application.Discounts.CreateCategoryDiscount;
using GameRa.Modules.Discounts.Application.Discounts.CreateGameDiscount;
using GameRa.Modules.Discounts.Application.Discounts.CreateGlobalDiscount;
using GameRa.Modules.Discounts.Application.Discounts.DeactivateDiscount;
using GameRa.Modules.Discounts.Domain;

namespace GameRa.Modules.Discounts.IntegrationTests.Abstractions;

internal static class CommandHelpers
{
    private static readonly Faker Faker = new();

    private static string Code(string prefix) =>
        $"{prefix}{Faker.Random.Guid():N}"[..Math.Min(12, prefix.Length + 8)].ToUpperInvariant();

    private static (DateTime Start, DateTime End) ActivePeriod(int days = 7)
    {
        DateTime start = DateTime.UtcNow.AddMinutes(-1);
        return (start, start.AddDays(days));
    }

    public static CreateGameDiscountCommand CreateGameDiscount(
        string? code = null,
        DiscountType? type = null,
        decimal? amount = null,
        Guid? gameId = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var period = ActivePeriod();
        return new(
            code ?? Code("GAME"),
            type ?? DiscountType.Percentage,
            amount ?? Faker.Random.Decimal(1, 50),
            gameId ?? Faker.Random.Guid(),
            startDate ?? period.Start,
            endDate ?? period.End);
    }

    public static CreateGlobalDiscountCommand CreateGlobalDiscount(
        string? code = null,
        DiscountType? type = null,
        decimal? amount = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var period = ActivePeriod(30);
        return new(
            code ?? Code("GLOBAL"),
            type ?? DiscountType.Percentage,
            amount ?? Faker.Random.Decimal(1, 50),
            startDate ?? period.Start,
            endDate ?? period.End);
    }

    public static CreateCategoryDiscountCommand CreateCategoryDiscount(
        string? code = null,
        DiscountType? type = null,
        decimal? amount = null,
        Guid? categoryId = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var period = ActivePeriod();
        return new(
            code ?? Code("CAT"),
            type ?? DiscountType.FixedAmount,
            amount ?? Faker.Random.Decimal(1, 50),
            categoryId ?? Faker.Random.Guid(),
            startDate ?? period.Start,
            endDate ?? period.End);
    }

    public static DeactivateDiscountCommand DeactivateDiscount(Guid discountId) =>
        new(discountId);

    public static ActivateDiscountCommand ActivateDiscount(Guid discountId) =>
        new(discountId);
}
