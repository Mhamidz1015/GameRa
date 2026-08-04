using GameRa.Common.Domain.Abstractions;
using System;

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

    public DateTime StartDateTimeUtc { get; private set; }

    public DateTime EndDateTimeUtc { get; private set; }

    public bool IsActive { get; private set; }

    public static Result<Discount> Create(
        string code,
        DiscountType type,
        decimal amount,
        DateTime startDateTimeUtc,
        DateTime endDateTimeUtc)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Result.Failure<Discount>(DiscountErrors.InvalidCode);
        }

        if (amount <= 0)
        {
            return Result.Failure<Discount>(DiscountErrors.InvalidAmount);
        }

        if (type == DiscountType.Percentage && amount > 100)
        {
            return Result.Failure<Discount>(DiscountErrors.InvalidPercentage);
        }

        var discount = new Discount
        {
            DiscountId = Guid.NewGuid(),
            Code = code.ToUpperInvariant(),
            Type = type,
            Amount = amount,
            StartDateTimeUtc = startDateTimeUtc,
            EndDateTimeUtc = endDateTimeUtc,
            IsActive = true
        };

        discount.Raise(new DiscountCreatedDomainEvent(discount.DiscountId));

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
}