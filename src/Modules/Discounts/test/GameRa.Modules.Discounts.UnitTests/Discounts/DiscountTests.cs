using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Discounts.Domain;
using GameRa.Modules.Discounts.UnitTests.Abstractions;

namespace GameRa.Modules.Discounts.UnitTests.Discounts;

public class DiscountTests : BaseTest
{
    // ─────────────────────────────────────────────
    // CreateForGame — Failure Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void CreateForGame_ShouldReturnFailure_WhenCodeIsEmpty()
    {
        Result<Discount> result = Discount.CreateForGame(
            string.Empty,
            DiscountType.Percentage,
            10m,
            Guid.NewGuid(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DiscountErrors.InvalidCode);
    }

    [Fact]
    public void CreateForGame_ShouldReturnFailure_WhenCodeIsWhitespace()
    {
        Result<Discount> result = Discount.CreateForGame(
            "   ",
            DiscountType.Percentage,
            10m,
            Guid.NewGuid(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DiscountErrors.InvalidCode);
    }

    [Fact]
    public void CreateForGame_ShouldReturnFailure_WhenAmountIsZero()
    {
        Result<Discount> result = Discount.CreateForGame(
            "SUMMER10",
            DiscountType.FixedAmount,
            0m,
            Guid.NewGuid(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DiscountErrors.InvalidAmount);
    }

    [Fact]
    public void CreateForGame_ShouldReturnFailure_WhenAmountIsNegative()
    {
        Result<Discount> result = Discount.CreateForGame(
            "SUMMER10",
            DiscountType.FixedAmount,
            -5m,
            Guid.NewGuid(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DiscountErrors.InvalidAmount);
    }

    [Fact]
    public void CreateForGame_ShouldReturnFailure_WhenPercentageExceeds100()
    {
        Result<Discount> result = Discount.CreateForGame(
            "SUMMER10",
            DiscountType.Percentage,
            101m,
            Guid.NewGuid(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DiscountErrors.InvalidPercentage);
    }

    // ─────────────────────────────────────────────
    // CreateForGame — Success Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void CreateForGame_ShouldSucceed_WithValidInputs()
    {
        Guid gameId = Guid.NewGuid();

        Result<Discount> result = Discount.CreateForGame(
            "summer10",
            DiscountType.Percentage,
            10m,
            gameId,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7));

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be("SUMMER10");
        result.Value.Scope.Should().Be(DiscountScope.Game);
        result.Value.GameId.Should().Be(gameId);
        result.Value.CategoryId.Should().BeNull();
        result.Value.IsActive.Should().BeTrue();
    }

    [Fact]
    public void CreateForGame_ShouldUppercaseCode()
    {
        Result<Discount> result = Discount.CreateForGame(
            "summer10",
            DiscountType.Percentage,
            10m,
            Guid.NewGuid(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7));

        result.Value.Code.Should().Be("SUMMER10");
    }

    [Fact]
    public void CreateForGame_ShouldRaiseDiscountCreatedDomainEvent()
    {
        Result<Discount> result = Discount.CreateForGame(
            "GAME10",
            DiscountType.Percentage,
            10m,
            Guid.NewGuid(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7));

        DiscountCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<DiscountCreatedDomainEvent>(result.Value);

        domainEvent.DiscountId.Should().Be(result.Value.DiscountId);
        domainEvent.Scope.Should().Be(DiscountScope.Game);
        domainEvent.GameId.Should().Be(result.Value.GameId);
        domainEvent.CategoryId.Should().BeNull();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(100)]
    public void CreateForGame_ShouldSucceed_WithValidPercentages(decimal percentage)
    {
        Result<Discount> result = Discount.CreateForGame(
            "PCT",
            DiscountType.Percentage,
            percentage,
            Guid.NewGuid(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7));

        result.IsSuccess.Should().BeTrue();
        result.Value.Amount.Should().Be(percentage);
    }

    // ─────────────────────────────────────────────
    // CreateForCategory — Success Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void CreateForCategory_ShouldSucceed_WithValidInputs()
    {
        Guid categoryId = Guid.NewGuid();

        Result<Discount> result = Discount.CreateForCategory(
            "CAT20",
            DiscountType.FixedAmount,
            20m,
            categoryId,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7));

        result.IsSuccess.Should().BeTrue();
        result.Value.Scope.Should().Be(DiscountScope.Category);
        result.Value.CategoryId.Should().Be(categoryId);
        result.Value.GameId.Should().BeNull();
    }

    [Fact]
    public void CreateForCategory_ShouldRaiseDiscountCreatedDomainEvent()
    {
        Guid categoryId = Guid.NewGuid();

        Result<Discount> result = Discount.CreateForCategory(
            "CAT20",
            DiscountType.FixedAmount,
            20m,
            categoryId,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7));

        DiscountCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<DiscountCreatedDomainEvent>(result.Value);

        domainEvent.Scope.Should().Be(DiscountScope.Category);
        domainEvent.CategoryId.Should().Be(categoryId);
        domainEvent.GameId.Should().BeNull();
    }

    // ─────────────────────────────────────────────
    // CreateGlobal — Success Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void CreateGlobal_ShouldSucceed_WithValidInputs()
    {
        Result<Discount> result = Discount.CreateGlobal(
            "GLOBAL50",
            DiscountType.Percentage,
            50m,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7));

        result.IsSuccess.Should().BeTrue();
        result.Value.Scope.Should().Be(DiscountScope.Global);
        result.Value.GameId.Should().BeNull();
        result.Value.CategoryId.Should().BeNull();
    }

    [Fact]
    public void CreateGlobal_ShouldRaiseDiscountCreatedDomainEvent()
    {
        Result<Discount> result = Discount.CreateGlobal(
            "GLOBAL50",
            DiscountType.Percentage,
            50m,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7));

        DiscountCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<DiscountCreatedDomainEvent>(result.Value);

        domainEvent.Scope.Should().Be(DiscountScope.Global);
        domainEvent.GameId.Should().BeNull();
        domainEvent.CategoryId.Should().BeNull();
    }

    // ─────────────────────────────────────────────
    // Activate — Failure Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void Activate_ShouldReturnFailure_WhenAlreadyActive()
    {
        Discount discount = CreateDefaultGameDiscount();

        Result result = discount.Activate();

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DiscountErrors.AlreadyActive);
    }

    // ─────────────────────────────────────────────
    // Activate — Success Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void Activate_ShouldSucceed_WhenDeactivated()
    {
        Discount discount = CreateDefaultGameDiscount();
        discount.Deactivate();
        discount.ClearDomainEvents();

        Result result = discount.Activate();

        result.IsSuccess.Should().BeTrue();
        discount.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Activate_ShouldRaiseDomainEvent_WhenSuccessful()
    {
        Discount discount = CreateDefaultGameDiscount();
        discount.Deactivate();
        discount.ClearDomainEvents();

        discount.Activate();

        DiscountActivatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<DiscountActivatedDomainEvent>(discount);

        domainEvent.DiscountId.Should().Be(discount.DiscountId);
    }

    // ─────────────────────────────────────────────
    // Deactivate — Failure Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void Deactivate_ShouldReturnFailure_WhenAlreadyDeactivated()
    {
        Discount discount = CreateDefaultGameDiscount();
        discount.Deactivate();

        Result result = discount.Deactivate();

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DiscountErrors.AlreadyDeactivated);
    }

    // ─────────────────────────────────────────────
    // Deactivate — Success Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void Deactivate_ShouldSucceed_WhenActive()
    {
        Discount discount = CreateDefaultGameDiscount();

        Result result = discount.Deactivate();

        result.IsSuccess.Should().BeTrue();
        discount.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Deactivate_ShouldRaiseDomainEvent_WhenSuccessful()
    {
        Discount discount = CreateDefaultGameDiscount();
        discount.ClearDomainEvents();

        discount.Deactivate();

        DiscountDeactivatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<DiscountDeactivatedDomainEvent>(discount);

        domainEvent.DiscountId.Should().Be(discount.DiscountId);
    }

    // ─────────────────────────────────────────────
    // CheckExpiration — Failure Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void CheckExpiration_ShouldReturnFailure_WhenExpired()
    {
        Discount discount = CreateExpiredDiscount();

        Result result = discount.CheckExpiration(DateTime.UtcNow);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DiscountErrors.Expired);
    }

    [Fact]
    public void CheckExpiration_ShouldDeactivate_WhenExpired()
    {
        Discount discount = CreateExpiredDiscount();

        discount.CheckExpiration(DateTime.UtcNow);

        discount.IsActive.Should().BeFalse();
    }

    [Fact]
    public void CheckExpiration_ShouldRaiseDomainEvent_WhenExpired()
    {
        Discount discount = CreateExpiredDiscount();
        discount.ClearDomainEvents();

        discount.CheckExpiration(DateTime.UtcNow);

        DiscountExpiredDomainEvent domainEvent =
            AssertDomainEventWasPublished<DiscountExpiredDomainEvent>(discount);

        domainEvent.DiscountId.Should().Be(discount.DiscountId);
    }

    // ─────────────────────────────────────────────
    // CheckExpiration — Success Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void CheckExpiration_ShouldSucceed_WhenNotExpired()
    {
        Discount discount = CreateDefaultGameDiscount();

        Result result = discount.CheckExpiration(DateTime.UtcNow);

        result.IsSuccess.Should().BeTrue();
        discount.IsActive.Should().BeTrue();
    }

    // ─────────────────────────────────────────────
    // Scope Isolation Tests
    // ─────────────────────────────────────────────

    [Fact]
    public void CreateForGame_ShouldSetScopeToGame_AndLeaveOtherScopesNull()
    {
        Result<Discount> result = Discount.CreateForGame(
            "GAME10", DiscountType.Percentage, 10m,
            Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddDays(7));

        result.Value.Scope.Should().Be(DiscountScope.Game);
        result.Value.GameId.Should().NotBeNull();
        result.Value.CategoryId.Should().BeNull();
    }

    [Fact]
    public void CreateForCategory_ShouldSetScopeToCategory_AndLeaveOtherScopesNull()
    {
        Result<Discount> result = Discount.CreateForCategory(
            "CAT10", DiscountType.Percentage, 10m,
            Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddDays(7));

        result.Value.Scope.Should().Be(DiscountScope.Category);
        result.Value.CategoryId.Should().NotBeNull();
        result.Value.GameId.Should().BeNull();
    }

    [Fact]
    public void CreateGlobal_ShouldSetScopeToGlobal_AndLeaveOtherScopesNull()
    {
        Result<Discount> result = Discount.CreateGlobal(
            "GLOBAL10", DiscountType.Percentage, 10m,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(7));

        result.Value.Scope.Should().Be(DiscountScope.Global);
        result.Value.GameId.Should().BeNull();
        result.Value.CategoryId.Should().BeNull();
    }

    // ─────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────

    private static Discount CreateDefaultGameDiscount() =>
        Discount.CreateForGame(
            "DEFAULT10",
            DiscountType.Percentage,
            10m,
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow.AddDays(7)).Value;

    private static Discount CreateExpiredDiscount() =>
        Discount.CreateForGame(
            "EXPIRED",
            DiscountType.Percentage,
            10m,
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(-10),
            DateTime.UtcNow.AddDays(-1)).Value;
}
