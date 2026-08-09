namespace GameRa.Modules.Store.IntegrationEvents;

public sealed class OrderCompletedGameModel
{
    public Guid GameId { get; init; }

    public string GameTitle { get; init; }

    public decimal FinalPrice { get; init; }
}
