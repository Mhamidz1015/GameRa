using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Store.Domain.Games;

public sealed class Game : Entity
{
    private Game()
    {
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public string Developer { get; private set; }

    public decimal BasePrice { get; private set; }

    public string Currency {  get; private set; }

    public string CoverImageUrl { get; private set; }

    public bool Delisteed { get; private set; }

    public static Game Create(
        Guid id,
        string title,
        string description,
        string developer,
        decimal basePrice,
        string coverImageUrl)
    {
        var game = new Game
        {
            Id = id,
            Title = title,
            Description = description,
            Developer = developer,
            BasePrice = basePrice,
            CoverImageUrl = coverImageUrl
        };

        return game;
    }

   
    public void Delist()
    {
        if (Delisteed)
        {
            return;
        }

        Delisteed = true;

        Raise(new GameDelistDomainEvent(Id));
    }

    public void PaymentsRefunded()
    {
        Raise(new GamePaymentsRefundedDomainEvent(Id));
    }
}
