using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Discounts.Domain;

public sealed class Discount : Entity
{
    private Discount()
    {
    }

    public Guid DiscountId { get; private set; }
    public string Code { get; private set; }

    public DiscountType Type { get; private set; }

    public decimal Amount { get; private set; }

    public DiscountScope Scope { get; private set; }

    public Guid? GameId { get; private set; }

    public Guid? CategoryId { get; private set; }

    public DateTime StartDateTimeUtc { get; private set; }

    public DateTime EndDateTimeUtc { get; private set; }

    public bool IsActive { get; private set; }

    public static Result<Discount> CreateForGame(
        string code,
        DiscountType type,
        decimal amount,
        Guid gameId,
        DateTime startDateTimeUtc,
        DateTime endDateTimeUtc)
    {
        Result validation = Validate(code, type, amount);
        if (validation.IsFailure)
        {
            return Result.Failure<Discount>(validation.Error);
        }

        var discount = new Discount
        {
            DiscountId = Guid.NewGuid(),
            Code = code.ToUpperInvariant(),
            Type = type,
            Amount = amount,
            Scope = DiscountScope.Game,
            GameId = gameId,
            CategoryId = null,
            StartDateTimeUtc = startDateTimeUtc,
            EndDateTimeUtc = endDateTimeUtc,
            IsActive = true
        };

        discount.Raise(new DiscountCreatedDomainEvent(
            discount.DiscountId,
            DiscountScope.Game,
            gameId,
            null));

        return discount;
    }

    public static Result<Discount> CreateForCategory(
        string code,
        DiscountType type,
        decimal amount,
        Guid categoryId,
        DateTime startDateTimeUtc,
        DateTime endDateTimeUtc)
    {
        Result validation = Validate(code, type, amount);
        if (validation.IsFailure)
        {
            return Result.Failure<Discount>(validation.Error);
        }

        var discount = new Discount
        {
            DiscountId = Guid.NewGuid(),
            Code = code.ToUpperInvariant(),
            Type = type,
            Amount = amount,
            Scope = DiscountScope.Category,
            GameId = null,
            CategoryId = categoryId,
            StartDateTimeUtc = startDateTimeUtc,
            EndDateTimeUtc = endDateTimeUtc,
            IsActive = true
        };

        discount.Raise(new DiscountCreatedDomainEvent(
            discount.DiscountId,
            DiscountScope.Category,
            null,
            categoryId));

        return discount;
    }

    public static Result<Discount> CreateGlobal(
        string code,
        DiscountType type,
        decimal amount,
        DateTime startDateTimeUtc,
        DateTime endDateTimeUtc)
    {
        Result validation = Validate(code, type, amount);
        if (validation.IsFailure)
        {
            return Result.Failure<Discount>(validation.Error);
        }

        var discount = new Discount
        {
            DiscountId = Guid.NewGuid(),
            Code = code.ToUpperInvariant(),
            Type = type,
            Amount = amount,
            Scope = DiscountScope.Global,
            GameId = null,
            CategoryId = null,
            StartDateTimeUtc = startDateTimeUtc,
            EndDateTimeUtc = endDateTimeUtc,
            IsActive = true
        };

        discount.Raise(new DiscountCreatedDomainEvent(
            discount.DiscountId,
            DiscountScope.Global,
            null,
            null));

        return discount;
    }

    public Result Activate()
    {
        if (IsActive)
        {
            return Result.Failure(DiscountErrors.AlreadyActive);
        }

        IsActive = true;
        Raise(new DiscountActivatedDomainEvent(DiscountId));

        return Result.Success();
    }

    public Result Deactivate()
    {
        if (!IsActive)
        {
            return Result.Failure(DiscountErrors.AlreadyDeactivated);
        }

        IsActive = false;
        Raise(new DiscountDeactivatedDomainEvent(DiscountId));

        return Result.Success();
    }

    public Result CheckExpiration(DateTime utcNow)
    {
        if (utcNow > EndDateTimeUtc)
        {
            IsActive = false;
            Raise(new DiscountExpiredDomainEvent(DiscountId));
            return Result.Failure(DiscountErrors.Expired);
        }

        return Result.Success();
    }

    private static Result Validate(string code, DiscountType type, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Result.Failure(DiscountErrors.InvalidCode);
        }

        if (amount <= 0)
        {
            return Result.Failure(DiscountErrors.InvalidAmount);
        }

        if (type == DiscountType.Percentage && amount > 100)
        {
            return Result.Failure(DiscountErrors.InvalidPercentage);
        }

        return Result.Success();
    }
}