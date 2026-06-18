using GameRa.Modules.Games.Domain.Abstractions;

namespace GameRa.Modules.Games.Domain.Games;

public sealed class Game : Entity
{
    private Game()
    {
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public  string Developer { get; private set; }

    public DateTime ReleaseDate { get; private set; }

    public decimal BasePrice { get; private set; }

    public string CoverImageUrl { get; private set; }

    public GameStatus Status { get; private set; }

    public static Game Create(
        string title,
        string description,
        string developer,
        DateTime releaseDate,
        decimal baseprice,
        string Coverimgageurl)
    {
        var Game = new Game
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            Developer = developer,
            ReleaseDate = releaseDate,
            BasePrice = baseprice,
            CoverImageUrl = Coverimgageurl
        };

        Game.Raise(new GameCreatedDomainEvent(Game.Id));

        return Game;
    }
}
