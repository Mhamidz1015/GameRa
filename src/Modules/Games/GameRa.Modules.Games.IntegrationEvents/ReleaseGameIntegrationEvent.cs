using GameRa.Common.Application.MessagingEventBus;

namespace GameRa.Modules.Games.IntegrationEvents;

public sealed class ReleaseGameIntegrationEvent : IntegrationEvent
{
    public ReleaseGameIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid gameId,
        string title,
        string description,
        string developer,
        DateTime releaseDate,
        string coverimgageurl,
        decimal baseprice)
        : base(id, occurredOnUtc)
    {
        GameId = gameId;
        Title = title;
        Description = description;
        Developer = developer;
        ReleaseDate = releaseDate;
        Coverimgageurl = coverimgageurl;
        Baseprice = baseprice;
    }

    public Guid GameId { get; init; }

    public string Title { get; init; }

    public string Description { get; init; }

    public string Developer { get; private set; }

    public DateTime ReleaseDate { get; private set; }

    public decimal Baseprice { get; private set; }

    public string Coverimgageurl { get; private set; }
}
