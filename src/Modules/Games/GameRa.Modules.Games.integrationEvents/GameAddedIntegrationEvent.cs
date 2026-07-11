using GameRa.Common.Application.MessagingEventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRa.Modules.Games.integrationEvents
{
    public sealed class GameAddedIntegrationEvent : IntegrationEvent
    {
        public GameAddedIntegrationEvent(
            Guid id,
            DateTime occurredOnUtc,
            Guid gameId,
            string title,
            string description,
            string developer,
            DateTime releaseDate,
            decimal baseprice,
            string Coverimageurl)
            : base(id, occurredOnUtc)
        {
            GameId = gameId;
            Title = title;
            Description = description;
            Developer = developer;
            BasePrice = baseprice;
            ReleaseDate = releaseDate;
            CoverImageUrl = Coverimageurl;
        }

        public Guid GameId { get; init; }

        public string Title { get; init; }

        public string Description { get; init; }

        public string Location { get; init; }

        public string Developer {  get; init; }

        public DateTime ReleaseDate { get; init; }

        public decimal BasePrice { get; init; }

        public string CoverImageUrl { get; init; }
    }

}
