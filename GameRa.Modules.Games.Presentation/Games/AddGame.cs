using GameRa.Modules.Games.Application.Games.AddGame;
using GameRa.Modules.Games.Presentation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Games.Presentation.Games;

internal static class AddGame
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("Games", async (Request request, ISender sender) =>
        {
            var command = new AddGameCommand(
                request.Title,
                request.Description,
                request.Developer,
                request.ReleaseDate,
                request.BasePrice,
                request.CoverImageUrl);

            Guid gameId = await sender.Send(command);

            return Results.Ok(gameId);
        })
        .WithTags(Tags.Games);
    }

    internal sealed class Request
    {
       public string Title { get; private set; }

        public string Description { get; private set; }

        public  string Developer { get; private set; }

        public DateTime ReleaseDate { get; private set; }

        public decimal BasePrice { get; private set; }

        public string CoverImageUrl { get; private set; }
    }
}
