using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Discounts.Domain;

public static class DiscountErrors
{
    public static Error NotFound(Guid discountId) => Error.NotFound(
        "Discount.NotFound",
        $"The discount with the specified identifier {discountId} was not found.");

    public static readonly Error InvalidCode = Error.Problem(
        "Discount.InvalidCode",
        "The discount code is invalid or empty.");

    public static readonly Error InvalidAmount = Error.Problem(
        "Discount.InvalidAmount",
        "The discount amount or percentage must be greater than zero.");

    public static readonly Error InvalidPercentage = Error.Problem(
        "Discount.InvalidPercentage",
        "Percentage discount cannot exceed 100%.");

    public static readonly Error AlreadyActive = Error.Problem(
        "Discount.AlreadyActive",
        "The discount is already active.");

    public static readonly Error AlreadyDeactivated = Error.Problem(
        "Discount.AlreadyDeactivated",
        "The discount is already deactivated.");

    public static readonly Error Expired = Error.Problem(
        "Discount.Expired",
        "The discount has expired.");
}