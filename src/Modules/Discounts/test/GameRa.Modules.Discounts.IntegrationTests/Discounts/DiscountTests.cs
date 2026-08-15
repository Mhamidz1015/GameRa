using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Discounts.IntegrationTests.Abstractions;
using GameRa.Modules.Discounts.Application.Discounts.GetActiveDiscountsForGame;
using GameRa.Modules.Discounts.Application.Discounts.GetDiscount;
using GameRa.Modules.Discounts.Application.Discounts.GetDiscountByCode;
using GameRa.Modules.Discounts.Domain;

namespace GameRa.Modules.Discounts.IntegrationTests.Discounts;

public sealed class DiscountTests : BaseIntegrationTest
{
    public DiscountTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    // ─────────────────────────────────────────────
    // CreateGameDiscount
    // ─────────────────────────────────────────────

    [Fact]
    public async Task CreateGameDiscount_ShouldSucceed_WhenInputsAreValid()
    {
        Result<Guid> result = await Sender.Send(CommandHelpers.CreateGameDiscount());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateGameDiscount_ShouldPersist_AndBeQueryable()
    {
        string code = $"GAME{Faker.Random.AlphaNumeric(6).ToUpper()}";
        Guid gameId = Faker.Random.Guid();

        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateGameDiscount(
            code, DiscountType.Percentage, 15m, gameId,
            null, null));

        Result<DiscountResponse> getResult = await Sender.Send(
            new GetDiscountQuery(createResult.Value));

        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.Code.Should().Be(code);
        getResult.Value.Amount.Should().Be(15m);
        getResult.Value.GameId.Should().Be(gameId);
        getResult.Value.Scope.Should().Be((int)DiscountScope.Game);
        getResult.Value.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task CreateGameDiscount_ShouldFail_WhenCodeAlreadyExists()
    {
        string code = $"DUPE{Faker.Random.AlphaNumeric(6).ToUpper()}";

        await Sender.Send(CommandHelpers.CreateGameDiscount(
            code, DiscountType.Percentage, 10m, Faker.Random.Guid(),
            null, null));

        Result<Guid> result = await Sender.Send(CommandHelpers.CreateGameDiscount(
            code, DiscountType.FixedAmount, 5m, Faker.Random.Guid(),
            null, null));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DiscountErrors.CodeAlreadyExists(code));
    }

    [Fact]
    public async Task CreateGameDiscount_ShouldFail_WhenCodeIsEmpty()
    {
        Result<Guid> result = await Sender.Send(CommandHelpers.CreateGameDiscount(code: string.Empty));

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task CreateGameDiscount_ShouldFail_WhenAmountIsZero()
    {
        Result<Guid> result = await Sender.Send(CommandHelpers.CreateGameDiscount(
            type: DiscountType.FixedAmount, amount: 0m));

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task CreateGameDiscount_ShouldFail_WhenPercentageExceeds100()
    {
        Result<Guid> result = await Sender.Send(CommandHelpers.CreateGameDiscount(
            amount: 101m));

        result.IsFailure.Should().BeTrue();
    }

    // ─────────────────────────────────────────────
    // CreateGlobalDiscount
    // ─────────────────────────────────────────────

    [Fact]
    public async Task CreateGlobalDiscount_ShouldSucceed_WhenInputsAreValid()
    {
        Result<Guid> result = await Sender.Send(CommandHelpers.CreateGlobalDiscount());

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task CreateGlobalDiscount_ShouldPersist_WithGlobalScope()
    {
        string code = $"GLOB{Faker.Random.AlphaNumeric(6).ToUpper()}";

        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateGlobalDiscount(
            code, DiscountType.Percentage, 20m,
            null, null));

        Result<DiscountResponse> getResult = await Sender.Send(
            new GetDiscountQuery(createResult.Value));

        getResult.Value.Scope.Should().Be((int)DiscountScope.Global);
        getResult.Value.GameId.Should().BeNull();
        getResult.Value.CategoryId.Should().BeNull();
    }

    // ─────────────────────────────────────────────
    // CreateCategoryDiscount
    // ─────────────────────────────────────────────

    [Fact]
    public async Task CreateCategoryDiscount_ShouldSucceed_WhenInputsAreValid()
    {
        Result<Guid> result = await Sender.Send(CommandHelpers.CreateCategoryDiscount());

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task CreateCategoryDiscount_ShouldPersist_WithCategoryScope()
    {
        Guid categoryId = Faker.Random.Guid();
        string code = $"CAT{Faker.Random.AlphaNumeric(6).ToUpper()}";

        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateCategoryDiscount(
            code, DiscountType.FixedAmount, 5m, categoryId,
            null, null));

        Result<DiscountResponse> getResult = await Sender.Send(
            new GetDiscountQuery(createResult.Value));

        getResult.Value.Scope.Should().Be((int)DiscountScope.Category);
        getResult.Value.CategoryId.Should().Be(categoryId);
        getResult.Value.GameId.Should().BeNull();
    }

    // ─────────────────────────────────────────────
    // GetDiscountByCode
    // ─────────────────────────────────────────────

    [Fact]
    public async Task GetDiscountByCode_ShouldReturnDiscount_WhenCodeExists()
    {
        string code = $"CODE{Faker.Random.AlphaNumeric(6).ToUpper()}";

        await Sender.Send(CommandHelpers.CreateGlobalDiscount(
            code, DiscountType.Percentage, 10m,
            null, null));

        Result<DiscountResponse> result = await Sender.Send(new GetDiscountByCodeQuery(code));

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(code);
    }

    [Fact]
    public async Task GetDiscountByCode_ShouldReturnFailure_WhenCodeDoesNotExist()
    {
        Result<DiscountResponse> result = await Sender.Send(
            new GetDiscountByCodeQuery("NONEXISTENT999"));

        result.IsFailure.Should().BeTrue();
    }

    // ─────────────────────────────────────────────
    // Activate / Deactivate
    // ─────────────────────────────────────────────

    [Fact]
    public async Task Deactivate_ShouldSucceed_WhenDiscountIsActive()
    {
        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateGlobalDiscount(
            $"DEACT{Faker.Random.AlphaNumeric(4).ToUpper()}",
            DiscountType.Percentage, 10m,
            null, null));

        Result result = await Sender.Send(CommandHelpers.DeactivateDiscount(createResult.Value));

        result.IsSuccess.Should().BeTrue();

        Result<DiscountResponse> getResult = await Sender.Send(
            new GetDiscountQuery(createResult.Value));

        getResult.Value.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task Activate_ShouldSucceed_WhenDiscountIsDeactivated()
    {
        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateGlobalDiscount(
            $"ACT{Faker.Random.AlphaNumeric(5).ToUpper()}",
            DiscountType.Percentage, 10m,
            null, null));

        await Sender.Send(CommandHelpers.DeactivateDiscount(createResult.Value));
        Result result = await Sender.Send(CommandHelpers.ActivateDiscount(createResult.Value));

        result.IsSuccess.Should().BeTrue();

        Result<DiscountResponse> getResult = await Sender.Send(
            new GetDiscountQuery(createResult.Value));

        getResult.Value.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Deactivate_ShouldFail_WhenAlreadyDeactivated()
    {
        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateGlobalDiscount(
            $"DACT{Faker.Random.AlphaNumeric(5).ToUpper()}",
            DiscountType.Percentage, 10m,
            null, null));

        await Sender.Send(CommandHelpers.DeactivateDiscount(createResult.Value));
        Result result = await Sender.Send(CommandHelpers.DeactivateDiscount(createResult.Value));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DiscountErrors.AlreadyDeactivated);
    }

    [Fact]
    public async Task Activate_ShouldFail_WhenAlreadyActive()
    {
        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateGlobalDiscount(
            $"AACT{Faker.Random.AlphaNumeric(5).ToUpper()}",
            DiscountType.Percentage, 10m,
            null, null));

        Result result = await Sender.Send(CommandHelpers.ActivateDiscount(createResult.Value));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DiscountErrors.AlreadyActive);
    }

    // ─────────────────────────────────────────────
    // GetActiveDiscountsForGame
    // ─────────────────────────────────────────────

    [Fact]
    public async Task GetActiveDiscountsForGame_ShouldReturnGameDiscount_WhenExists()
    {
        Guid gameId = Faker.Random.Guid();
        Guid categoryId = Faker.Random.Guid();

        await Sender.Send(CommandHelpers.CreateGameDiscount(
            $"GD{Faker.Random.AlphaNumeric(6).ToUpper()}",
            DiscountType.Percentage, 10m, gameId,
            null, null));

        Result<IReadOnlyCollection<DiscountResponse>> result = await Sender.Send(
            new GetActiveDiscountsForGameQuery(gameId, categoryId));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Contain(d => d.GameId == gameId);
    }

    [Fact]
    public async Task GetActiveDiscountsForGame_ShouldReturnGlobalDiscount()
    {
        Guid gameId = Faker.Random.Guid();
        Guid categoryId = Faker.Random.Guid();

        await Sender.Send(CommandHelpers.CreateGlobalDiscount(
            $"GL{Faker.Random.AlphaNumeric(6).ToUpper()}",
            DiscountType.Percentage, 5m,
            null, null));

        Result<IReadOnlyCollection<DiscountResponse>> result = await Sender.Send(
            new GetActiveDiscountsForGameQuery(gameId, categoryId));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Contain(d => d.Scope == (int)DiscountScope.Global);
    }

    [Fact]
    public async Task GetActiveDiscountsForGame_ShouldNotReturn_DeactivatedDiscounts()
    {
        Guid gameId = Faker.Random.Guid();
        Guid categoryId = Faker.Random.Guid();

        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateGameDiscount(
            $"DEACTGAME{Faker.Random.AlphaNumeric(3).ToUpper()}",
            DiscountType.Percentage, 10m, gameId,
            null, null));

        await Sender.Send(CommandHelpers.DeactivateDiscount(createResult.Value));

        Result<IReadOnlyCollection<DiscountResponse>> result = await Sender.Send(
            new GetActiveDiscountsForGameQuery(gameId, categoryId));

        result.Value.Should().NotContain(d => d.DiscountId == createResult.Value);
    }
}
