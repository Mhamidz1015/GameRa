using GameRa.Common.Domain.Abstractions;

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

    public static Result<Game> Create(
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
        {
            return Result.Failure(GameErrors.Released);
        }

        Status = GameStatus.Released;

        Raise(new GameReleasedDomainEvent(Id));

        return Result.Success();
    }
    public Result Delist()
    {
        if (Status == GameStatus.Delisted)
        {
            return Result.Failure(GameErrors.AlreadyDelisted);
        }

        Status = GameStatus.Delisted;

        Raise(new GameDelistedDomainEvent(Id));

        return Result.Success();

    }

}
