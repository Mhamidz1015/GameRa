using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Domain.Categories;

namespace GameRa.Modules.Games.Domain.Games;

public sealed class Game : Entity
{
    private Game()
    {
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public string Developer { get; private set; }

    public DateTime ReleaseDate { get; private set; }

    public decimal BasePrice { get; private set; }

    public string CoverImageUrl { get; private set; }

    public GameStatus Status { get; private set; }

    public Guid CategoryId { get; private set; }

    // Read model fields — updated via Integration Events
    public decimal? ActiveDiscountAmount { get; private set; }

    public bool? IsDiscountPercentage { get; private set; }

    public double AverageRating { get; private set; }

    public int TotalReviews { get; private set; }

    public decimal CurrentPrice
    {
        get
        {
            if (ActiveDiscountAmount is null)
                return BasePrice;

            if (IsDiscountPercentage == true)
                return BasePrice - (BasePrice * ActiveDiscountAmount.Value / 100);

            return Math.Max(0, BasePrice - ActiveDiscountAmount.Value);
        }
    }

    public static Result<Game> Create(
        Guid categoryId,
        string title,
        string description,
        string developer,
        DateTime releaseDate,
        decimal basePrice,
        string coverImageUrl)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<Game>(GameErrors.TitleIsEmpty);

        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Game>(GameErrors.DescriptionIsEmpty);

        if (basePrice < 0)
            return Result.Failure<Game>(GameErrors.PriceCannotBeNegative);

        var game = new Game
        {
            Id = Guid.NewGuid(),
            CategoryId = categoryId,
            Title = title,
            Description = description,
            Developer = developer,
            ReleaseDate = releaseDate,
            BasePrice = basePrice,
            CoverImageUrl = coverImageUrl
        };

        game.Raise(new GameAddedDomainEvent(game.Id));

        return Result.Success(game);
    }

    public Result Release()
    {
        if (Status != GameStatus.ComingSoon)
            return Result.Failure(GameErrors.Released);

        Status = GameStatus.Released;
        Raise(new GameReleasedDomainEvent(Id));

        return Result.Success();
    }

    public Result Delist()
    {
        if (Status == GameStatus.Delisted)
            return Result.Failure(GameErrors.AlreadyDelisted);

        Status = GameStatus.Delisted;
        Raise(new GameDelistedDomainEvent(Id));

        return Result.Success();
    }

    public void ApplyDiscount(decimal amount, bool isPercentage)
    {
        ActiveDiscountAmount = amount;
        IsDiscountPercentage = isPercentage;
    }

    public void RemoveDiscount()
    {
        ActiveDiscountAmount = null;
        IsDiscountPercentage = null;
    }

    public void UpdateRating(int newRating)
    {
        double totalScore = AverageRating * TotalReviews + newRating;
        TotalReviews++;
        AverageRating = totalScore / TotalReviews;
    }

    public void RemoveRating(int oldRating)
    {
        if (TotalReviews <= 1)
        {
            AverageRating = 0;
            TotalReviews = 0;
            return;
        }

        double totalScore = AverageRating * TotalReviews - oldRating;
        TotalReviews--;
        AverageRating = totalScore / TotalReviews;
    }
}
