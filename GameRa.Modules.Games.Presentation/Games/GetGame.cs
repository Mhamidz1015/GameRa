using GameRa.Modules.Games.Application.Games.GetGame;
using GameRa.Modules.Games.Presentation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GameRa.Modules.Games.Presentation.Games;

internal static class GetGame
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("Game/{id}", async (Guid id, ISender sender) =>
        {
            GameResponse game = await sender.Send(new GetGameQuery(id));

            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithTags(Tags.Games);
    }
}
